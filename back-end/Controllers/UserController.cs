using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

[Route("api/user")]
[ApiController]
[Authorize]
public class UserController(AppDbContext dbContext) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly Serilog.ILogger _logger = Log.ForContext<AuthController>();

    [HttpPost("updatePremium")]
    public async Task<IActionResult> UpdatePremium()
    {
        var GetUserResult = await UserHelper.GetCurrentUserAsync(User, _dbContext, _logger);
        if (GetUserResult.IsFailed)
            return Unauthorized(GetUserResult.Errors.First().Message);

        var user = await _dbContext.Users
            .Where(u => u.Id == GetUserResult.Value.Id)
            .FirstOrDefaultAsync();

        if(user == null)
        {
            _logger.Error($"User not found with ID: '{GetUserResult.Value.Id}'.");
            return NotFound();
        }

        user.Premium = true;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        Response.Cookies.Append("UserType", "premium", new CookieOptions
        {
            HttpOnly = false,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddHours(9)
        });

        return Ok();
    }

    [HttpGet("solutions")]
    public async Task<IActionResult> Solutions()
    {
        var GetUserResult = await UserHelper.GetCurrentUserAsync(User, _dbContext, _logger);
        if (GetUserResult.IsFailed)
            return Unauthorized(GetUserResult.Errors.First().Message);

        var userId = GetUserResult.Value.Id;

        var lessonScores = await (
            from lesson in _dbContext.Lessons
            join problem in _dbContext.Problems on lesson.Id equals problem.LessonId into lessonProblems
            from problem in lessonProblems.DefaultIfEmpty()

            join submission in _dbContext.Submissions
                .Where(s => s.UserId == userId)
                on problem.Id equals submission.ProblemId into problemSubmissions
            from submission in problemSubmissions.DefaultIfEmpty()

            select new
            {
                LessonTitle = lesson.Title,
                Score = submission != null ? submission.Score : 0
            }
        ).ToListAsync();

        return Ok(lessonScores);
    }
}
