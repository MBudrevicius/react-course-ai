using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Db;
using Models.Request;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config, AppDbContext dbContext)
    {
        _config = config;
        _dbContext = dbContext;
    }

   [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        string normalizedEmail = request.Email.Trim().ToLower();
        string normalizedUsername = request.Username.Trim();

        bool emailExists = await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);
        if (emailExists) return BadRequest("Email is already registered.");

        bool usernameExists = await _dbContext.Users.AnyAsync(u => u.Username.ToLower() == normalizedUsername.ToLower());
        if (usernameExists) return BadRequest("Username is already taken.");

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
            return Unauthorized("Invalid username or email.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid password.");

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
            Expires = DateTime.UtcNow.AddHours(1)
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
