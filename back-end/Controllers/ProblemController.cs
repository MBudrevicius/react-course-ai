
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

[Route("api/problems")]
[ApiController]
[Authorize]
public class ProblemController(AppDbContext dbContext) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly Serilog.ILogger _logger = Log.ForContext<ProblemController>();

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetProblems(int lessonId)
    {
        var GetUserResult = await UserHelper.GetCurrentUserAsync(User, _dbContext, _logger);
        if (GetUserResult.IsFailed)
            return Unauthorized(GetUserResult.Errors.First().Message);

        var lesson = await _dbContext.Lessons
            .AsNoTracking()
            .Where(l => l.Id == lessonId)
            .FirstOrDefaultAsync();

        if(lesson == null)
        {
            _logger.Error($"Lesson not found with ID: '{lessonId}'.");
            return NotFound();
        }

        if(lesson.Premium && !GetUserResult.Value.Premium)
        {
            _logger.Error($"Non premium user with ID: '{GetUserResult.Value.Id}' tried to access premium lesson: '{lesson.Title}'.");
            return Forbid("This lesson is only available for premium users.");
        }

        var problems = await _dbContext.Problems
            .AsNoTracking()
            .Where(t => t.LessonId == lessonId)
            .ToListAsync();

        return Ok(problems);
    }

    [HttpGet("bestSubmission/{problemId}")]
    public async Task<IActionResult> GetBestSubmission(int problemId)
    {
        var GetUserResult = await UserHelper.GetCurrentUserAsync(User, _dbContext, _logger);
        if (GetUserResult.IsFailed)
        {
            return Unauthorized(GetUserResult.Errors.First().Message);
        }

        var bestSubmission = await _dbContext.Submissions
            .AsNoTracking()
            .Where(s => s.UserId == GetUserResult.Value.Id && s.ProblemId == problemId)
            .FirstOrDefaultAsync();
        if (bestSubmission == null)
        {
            _logger.Error($"No submissions found for user ID: '{GetUserResult.Value.Id}' and problem ID: '{problemId}'.");
            return NotFound("No submissions found for this user and task.");
        }

        return Ok(bestSubmission);
    }
}