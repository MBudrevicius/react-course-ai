using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Request;
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
            .OrderBy(l => l.OrderIndex)
            .Select(l => new { l.Id, l.Title })
            .ToListAsync();
        return Ok(lessonTitles);
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetLesson(int lessonId)
    {
        var lesson = await _dbContext.Lessons
            .Where(l => l.Id == lessonId)
            .FirstOrDefaultAsync();

        if (lesson == null)
        {
            _logger.Error($"Lesson not found with ID: '{lessonId}'.");
            return NotFound();
        }

        return Ok(lesson);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddLesson([FromBody] LessonRequest lesson)
    {
        int lastOrderIndex = await _dbContext.Lessons.MaxAsync(l => (int?)l.OrderIndex) ?? 0;
        var newLesson = new Lesson
        {
            Title = lesson.Title,
            Content = lesson.Content,
            OrderIndex = lastOrderIndex + 1
        };

        _dbContext.Lessons.Add(newLesson);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLesson), new { lessonId = newLesson.Id }, newLesson);
    }
}
