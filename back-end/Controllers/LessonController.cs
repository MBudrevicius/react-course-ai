using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

[Route("api/lessons")]
[ApiController]
[Authorize]
public class LessonController(AppDbContext dbContext) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly Serilog.ILogger _logger = Log.ForContext<LessonController>();

    [HttpGet("titles")]
    public async Task<IActionResult> GetTitles()
    {
        var lessonTitles = await _dbContext.Lessons
            .AsNoTracking()
            .OrderBy(l => l.OrderIndex)
            .Select(l => new { l.Id, l.Title, l.Premium })
            .ToListAsync();
        return Ok(lessonTitles);
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetLesson(int lessonId)
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

        return Ok(lesson);
    }
}
