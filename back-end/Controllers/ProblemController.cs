
using System.Security.Claims;
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

    [HttpGet("bestSubmission/{problemId}")]
    public async Task<IActionResult> GetBestSubmission(int problemId)
    {
        string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            Console.WriteLine("Invalid user token. " + userIdClaim);
            return Unauthorized("Invalid user token.");
        }

        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            Console.WriteLine("User not found. " + userId);
            return NotFound("User not found.");
        }

        var bestSubmission = await _dbContext.Submissions
            .Where(s => s.UserId == userId && s.ProblemId == problemId)
            .FirstOrDefaultAsync();

        if (bestSubmission == null)
        {
            Console.WriteLine("No submissions found for this user and task. " + userId);
            return NotFound("No submissions found for this user and task.");
        }

        return Ok(bestSubmission);
    }
}