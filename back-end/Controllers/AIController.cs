
using System.Security.Claims;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Request;
using OpenAI.Chat;

[Route("api/ai")]
[ApiController]
[Authorize]
public class AIController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _dbContext;
    private readonly string _openAiApiKey;

    public AIController(IConfiguration config, AppDbContext dbContext)
    {
        _config = config;
        _dbContext = dbContext;
        _openAiApiKey = _config["OpenAI:ApiKey"];
    }

    [HttpPost("evaluate/{problemId}")]
    public async Task<IActionResult> EvaluateSolution(int problemId, [FromBody] CodeSubmissionRequest request)
    {
        string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid user token.");
        }

        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return NotFound("User not found.");

        var problem = await _dbContext.Problems.AsNoTracking().FirstOrDefaultAsync(t => t.Id == problemId);
        if (problem == null) return NotFound("Problem not found.");

        string prompt = "Tau bus pateikta užduotis ir naudotojo įkeltas sprendimo kodas. Įvertink sprendimą balu tarp 0-100 ir duok grįžtamąjį ryšį. Pateik atsakymą formatu: \"{score} {newline} {feedback}\"\n\n" +
                        $"Užduotis: \"{problem.Question}\"\n\n" +
                        $"Naudotojo sprendimas: {request.CodeSubmission}\n\n";

        var response = await GetAiResponseAsync(_config["OpenAI:ModelName"], "You are a React code evaluator.", prompt);

        if (string.IsNullOrWhiteSpace(response))
        {
            return StatusCode(500, "Error processing AI response.");
        }

        var (score, feedback) = ParseScoreAndFeedback(response);

        var bestAttempt = await _dbContext.Submissions
            .Where(s => s.UserId == userId && s.ProblemId == problemId)
            .FirstOrDefaultAsync();

        if (bestAttempt == null || score > bestAttempt.Score)
        {
            if (bestAttempt != null)
            {
                bestAttempt.Code = request.CodeSubmission;
                bestAttempt.Score = score;
                bestAttempt.Feedback = feedback;
                _dbContext.Submissions.Update(bestAttempt);
            }
            else
            {
                var newSubmission = new CodeSubmission
                {
                    UserId = userId,
                    ProblemId = problemId,
                    Code = request.CodeSubmission,
                    Score = score,
                    Feedback = feedback,
                };
                _dbContext.Submissions.Add(newSubmission);
            }

            await _dbContext.SaveChangesAsync();
        }

        return Ok(new { score, feedback });
    }

    private (int score, string feedback) ParseScoreAndFeedback(string response)
    {
        var lines = response.Split('\n');
        int score = int.Parse(lines[0]);
        string feedback = string.Join("\n", lines, 1, lines.Length - 1);
        return (score, feedback);
    }
    
    [HttpPost("chat")]
    public async Task<IActionResult> ChatWithGPT([FromBody] ChatRequest request)
    {
        var response = await GetAiResponseAsync("gpt-4o", "You are a React coding assistant, helping a person learning React.", request.Message);
        
        if (string.IsNullOrWhiteSpace(response))
        {
            return StatusCode(500, "Error: No response from AI.");
        }

        return Ok(new { response });
    }

    private async Task<string?> GetAiResponseAsync(string aiModelName, string systemMessage, string userMessage)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemMessage),
            new UserChatMessage(userMessage)
        };

        var openAiClient = new ChatClient(aiModelName, _openAiApiKey);

        var response = await openAiClient.CompleteChatAsync(messages);
        return response?.Value?.Content?[0]?.Text;
    }
}
