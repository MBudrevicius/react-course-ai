
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Request;

[Route("api/problems")]
[ApiController]
[Authorize]
public class ProblemController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ProblemController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetProblems(int lessonId)
    {
        var problems = await _dbContext.Problems
            .Where(t => t.LessonId == lessonId)
            .ToListAsync();

        return Ok(problems);
    }

    [HttpPost("{lessonId}")]
    public async Task<IActionResult> AddProblem(int lessonId, [FromBody] ProblemRequest problem)
    {
        var lesson = await _dbContext.Lessons.FindAsync(lessonId);
        if (lesson == null)
        {
            return NotFound("Lesson not found.");
        }

        var lastOrderIndex = await _dbContext.Problems
            .Where(t => t.LessonId == lessonId)
            .MaxAsync(t => (int?)t.OrderIndex) ?? 0;

        var newProblem = new Problem
        {
            LessonId = lessonId,
            OrderIndex = lastOrderIndex + 1,
            Question = problem.Question
        };

        _dbContext.Problems.Add(newProblem);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProblems), new { lessonId }, newProblem);
    }

    [HttpPost("bestSubmission")]
    public async Task<IActionResult> GetBestSubmission([FromBody] BestSubmissionRequest request)
    {
        var bestSubmission = await _dbContext.Submissions
            .Where(s => s.UserId == request.UserId && s.ProblemId == request.ProblemId)
            .FirstOrDefaultAsync();

        if (bestSubmission == null)
        {
            return NotFound("No submissions found for this user and task.");
        }

        return Ok(bestSubmission);
    }
}