using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Db;
using Models.Request;
using Serilog;

[Route("api/auth")]
[ApiController]
public class AuthController(IConfiguration config, AppDbContext dbContext) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IConfiguration _config = config;
    private readonly Serilog.ILogger _logger = Log.ForContext<AuthController>();

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        string normalizedEmail = request.Email.Trim().ToLower();
        bool emailExists = await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);
        if (emailExists)
        {
            _logger.Error($"User tried to register with already existing email: '{normalizedEmail}'.");
            return BadRequest("Šis el. paštas jau naudojamas.");
        }

        string normalizedUsername = request.Username.Trim();
        bool usernameExists = await _dbContext.Users.AnyAsync(u => u.Username.ToLower() == normalizedUsername.ToLower());
        if (usernameExists)
        {
            _logger.Error($"User tried to register with already existing username: '{normalizedUsername}'.");
            return BadRequest("Šis vartotojo vardas jau naudojamas.");
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var newUser = new User
        {
            Username = normalizedUsername,
            Email = normalizedEmail,
            PasswordHash = hashedPassword,
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return LoginUser(newUser, "Registration successful");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        string usernameOrEmail = request.UsernameOrEmail.Trim().ToLower();

        var user = await _dbContext.Users
            .Where(u => u.Email.ToLower() == usernameOrEmail || u.Username.ToLower() == usernameOrEmail)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            _logger.Error($"During login User was not found for: '{usernameOrEmail}'.");
            return Unauthorized("Invalid username or email.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.Error($"During login User provided invalid password for: '{usernameOrEmail}'.");
            return Unauthorized("Invalid password.");
        }

        return LoginUser(user, "Login successful");
    }

    private IActionResult LoginUser(User user, string message)
    {
        var token = GenerateJwtToken(user);

        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = false,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddHours(9)
        });

        return Ok(new { message, token });
    }

    private string GenerateJwtToken(User user)
    {
        string? secretKey = _config["Jwt:Secret"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT Secret key is missing from configuration.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("username", user.Username)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
