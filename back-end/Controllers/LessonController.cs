using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Request;

[Route("api/lessons")]
[ApiController]
[Authorize]
public class LessonController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public LessonController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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
