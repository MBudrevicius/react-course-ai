
using System.Security.Claims;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Request;
using OpenAI.Chat;
using OpenAI.Audio;
using Serilog;

[Route("api/ai")]
[ApiController]
[Authorize]
public class AIController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _dbContext;
    private readonly string _openAiApiKey;
    private readonly Serilog.ILogger _logger;

    public AIController(IConfiguration config, AppDbContext dbContext)
    {
        _config = config;
        _dbContext = dbContext;
        _openAiApiKey = _config["OpenAI:ApiKey"];
        _logger = Log.ForContext<AIController>();
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

        if (string.IsNullOrWhiteSpace(response.Item1))
        {
            return StatusCode(500, "Error processing AI response.");
        }

        var (score, feedback) = ParseScoreAndFeedback(response.Item1);

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
        var response = await GetAiResponseAsync("gpt-4o", "You are a React coding assistant, helping a person learning React.", request.Message, request.ContextId);
        
        if (string.IsNullOrWhiteSpace(response.Item1))
        {
            return StatusCode(500, "Error: No response from AI.");
        }

        return Ok(new { reply = response.Item1, contextId = response.Item2 });
    }

    [HttpPost("transcribe")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AudioChat([FromForm] TranscribeRequest request)
    {
        var transcription = await TranscribeAudio(request.Audio);

        _logger.Warning("Transcription: {Transcription}", transcription);

        var response = await GetAiResponseAsync("gpt-4o", "You are a React coding assistant, helping a person learning React.", transcription, request.ContextId);
        
        if (string.IsNullOrWhiteSpace(response.Item1))
        {
            return StatusCode(500, "Error: No response from AI.");
        }

        return Ok(new { reply = response.Item1, contextId = response.Item2 });
    }

    private enum ChatMessageType
    {
        System = 0,
        User = 1,
        Assistant = 2
    }

    private async Task<string> TranscribeAudio(IFormFile audio)
    {
        Stream fileStream = audio.OpenReadStream();

        var openAi = new AudioClient("whisper-1", _openAiApiKey);
        var audioOptions = new AudioTranscriptionOptions()
        {
            Language = "lt",
            ResponseFormat = AudioTranscriptionFormat.Text,
        };

        var audioClient = new AudioClient("whisper-1", _openAiApiKey);

        var response = await audioClient.TranscribeAudioAsync(fileStream, audio.FileName, audioOptions);

        return response.Value.Text;
    }

    private async Task<(string?, int)> GetAiResponseAsync(string aiModelName, string systemMessage, string userMessage, int? contextId = null)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemMessage)
        };
        var nextIndex = 0;

        if (contextId != null)
        {
            var history = await _dbContext.ChatHistory
                .Where(h => h.ContextId == contextId)
                .OrderBy(h => h.OrderIndex)
                .ToListAsync();
            foreach (var h in history)
            {
                switch ((ChatMessageType)h.MessageType)
                {
                    case ChatMessageType.System:
                        messages.Add(new SystemChatMessage(h.Message));
                        break;
                    case ChatMessageType.User:
                        messages.Add(new UserChatMessage(h.Message));
                        break;
                    case ChatMessageType.Assistant:
                        messages.Add(new AssistantChatMessage(h.Message));
                        break;
                }
            }
            nextIndex = history.Count > 0 ? history.Max(h => h.OrderIndex) + 1 : 0;
        }
        else
        {
            contextId = await _dbContext.ChatHistory.MaxAsync(t => t.ContextId) + 1;
        }

        messages.Add(new UserChatMessage(userMessage));
        _dbContext.ChatHistory.Add(new ChatHistory
        {
            ContextId = (int)contextId,
            OrderIndex = nextIndex,
            MessageType = (int)ChatMessageType.User,
            Message = userMessage
        });

        var openAiClient = new ChatClient(aiModelName, _openAiApiKey);
        var response = await openAiClient.CompleteChatAsync(messages);
        var assistantResponse = response?.Value?.Content?[0]?.Text;
        if (!string.IsNullOrWhiteSpace(assistantResponse))
        {
            messages.Add(new AssistantChatMessage(assistantResponse));

            _dbContext.ChatHistory.Add(new ChatHistory
            {
                ContextId = (int)contextId,
                OrderIndex = nextIndex + 1,
                MessageType = (int)ChatMessageType.Assistant,
                Message = assistantResponse
            });

            await _dbContext.SaveChangesAsync();
        }

        return (assistantResponse, (int)contextId);
    }
}
