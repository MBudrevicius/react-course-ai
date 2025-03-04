using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using RequestModels;
using DbModels;

[Route("lessons")]
[ApiController]
[Authorize]
public class LessonController(AppDbContext dbContext) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;

    [HttpGet("titles")]
    public async Task<IActionResult> GetLessonTitles()
    {
        var lessonTitles = await _dbContext.Lessons
            .OrderBy(l => l.OrderIndex)
            .Select(l => new { l.Id, l.Title })
            .ToListAsync();

        return Ok(lessonTitles);
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetLessonById(int lessonId)
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
        if (string.IsNullOrWhiteSpace(lesson.Title) || string.IsNullOrWhiteSpace(lesson.Content))
        {
            return BadRequest("Lesson title and content are required.");
        }

        int lastOrderIndex = await _dbContext.Lessons.MaxAsync(l => (int?)l.OrderIndex) ?? 0;
        var newLesson = new Lesson
        {
            Title = lesson.Title,
            Content = lesson.Content,
            OrderIndex = lastOrderIndex + 1
        };

        _dbContext.Lessons.Add(newLesson);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLessonById), new { lessonId = newLesson.Id }, newLesson);
    }

    [HttpGet("{lessonId}/tasks")]
    public async Task<IActionResult> GetTasksForLesson(int lessonId)
    {
        var tasks = await _dbContext.Tasks
            .Where(t => t.LessonId == lessonId)
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpPost("{lessonId}/tasks")]
    public async Task<IActionResult> AddTaskToLesson(int lessonId, [FromBody] TaskRequest task)
    {
        var lesson = await _dbContext.Lessons.FindAsync(lessonId);
        if (lesson == null)
            return NotFound("Lesson not found.");

        var lastOrderIndex = await _dbContext.Tasks
            .Where(t => t.LessonId == lessonId)
            .MaxAsync(t => (int?)t.OrderIndex) ?? 0;
        var newTask = new DbModels.Task
        {
            LessonId = lessonId,
            OrderIndex = lastOrderIndex + 1,
            Question = task.Question
        };

        _dbContext.Tasks.Add(newTask);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTasksForLesson), new { lessonId = newTask.Id }, newTask);
    }
}
