using Data;
using FluentResults;
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
public class AIController(IConfiguration config, AppDbContext dbContext) : ControllerBase
{
    private readonly string _openAiApiKey = config["OpenAI:ApiKey"] ?? string.Empty;
    private readonly string _openAiModelName = config["OpenAI:ModelName"] ?? string.Empty;
    private readonly AppDbContext _dbContext = dbContext;
    private readonly Serilog.ILogger _logger = Log.ForContext<AIController>();

    [HttpPost("evaluate/{problemId}")]
    public async Task<IActionResult> EvaluateCode(int problemId, [FromBody] EvaluateCodeRequest request)
    {
        var GetUserResult = await UserHelper.GetCurrentUserAsync(User, _dbContext, _logger);
        if (GetUserResult.IsFailed)
            return Unauthorized(GetUserResult.Errors.First().Message);

        var problem = await _dbContext.Problems
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == problemId);
        if (problem == null)
        {
            _logger.Error($"Problem not found with ID: '{problemId}'.");
            return NotFound("Problem not found");
        }

        string[] systemMessages =
        [
            "You are a React code evaluator. You will be given a task and a user's code submission. Evaluate the code and provide score and feedback. Score has to be between 0-100. When evaluating check if code is valid React code, compilable and provide feedback on the code quality, correctness, and any improvements that can be made.",
            "Evaluation message format has to be: \"{score} {newline} {feedback}\"",
            "Your response has to be in Lithuanian language.",
            $"The React task that the code will have to solve is (it's in Lithuanian language): \"{problem.Question}\"",
        ];
        string prompt = $"Evaluate the solution: {request.CodeSubmission}";

        var response = await GetAiResponseAsync(_openAiModelName, systemMessages, prompt);
        if (string.IsNullOrWhiteSpace(response.Item1))
        {
            _logger.Error("OpenAI response for code evaluation was empty or null.");
            return StatusCode(500, "Error while processing OpenAI response. Try again later.");
        }

        _logger.Information($"OpenAI response for code evaluation: '{response.Item1}'");
        var parseResult = ParseScoreAndFeedback(response.Item1);
        if (parseResult.IsFailed)
        {
            _logger.Error($"OpenAI response parsing failed: {parseResult.Errors.First().Message}");
            return StatusCode(500, "Error while parsing OpenAI response. Try again later.");
        }

        var bestAttempt = await _dbContext.Submissions
            .Where(s => s.UserId == GetUserResult.Value.Id && s.ProblemId == problemId)
            .FirstOrDefaultAsync();
        if (bestAttempt == null || parseResult.Value.score > bestAttempt.Score)
        {
            if (bestAttempt != null)
            {
                bestAttempt.Code = request.CodeSubmission;
                bestAttempt.Score = parseResult.Value.score;
                bestAttempt.Feedback = parseResult.Value.feedback;
                _dbContext.Submissions.Update(bestAttempt);
            }
            else
            {
                var newSubmission = new CodeSubmission
                {
                    UserId = GetUserResult.Value.Id,
                    ProblemId = problemId,
                    Code = request.CodeSubmission,
                    Score = parseResult.Value.score,
                    Feedback = parseResult.Value.feedback,
                };
                _dbContext.Submissions.Add(newSubmission);
            }

            await _dbContext.SaveChangesAsync();
        }

        return Ok(new { parseResult.Value.score, parseResult.Value.feedback });
    }

    private static Result<(int score, string feedback)> ParseScoreAndFeedback(string response)
    {
        var lines = response.Split('\n');
        if(!int.TryParse(lines[0], out int score))
        {
            return Result.Fail($"AI response had wrong format and couldn't get score. (AI response: '{response}').");
        }

        string feedback = string.Join("\n", lines, 1, lines.Length - 1);
        return (score, feedback);
    }

    [HttpPost("chat")]
    public async Task<IActionResult> ChatWithGPT([FromBody] ChatRequest request)
    {
        var lessons = await _dbContext.Lessons
            .AsNoTracking()
            .Select(l => l.Title)
            .ToListAsync();

        string[] systemMessages =
        [
            "You are a React coding assistant for a person going through React course page.",
            "The course is divided into lessons. Each lesson titles are: '" + string.Join(", ", lessons) + "'",
            "Your response has to be in Lithuanian language.",
        ];
        var systemPrompt = systemMessages
            .Select(message => new SystemChatMessage(message))
            .Cast<ChatMessage>()
            .ToList();

        var response = await GetAiResponseAsync("gpt-4o", systemMessages, request.Message, request.ContextId);
        if (string.IsNullOrWhiteSpace(response.Item1))
        {
            _logger.Error($"OpenAI didn't respond to user chat message. (Chat message: '{request.Message}')");
            return StatusCode(500, "Error while processing OpenAI response. Try again later.");
        }

        _logger.Information($"OpenAI response for chat message: '{response.Item1}'");

        return Ok(new { reply = response.Item1, contextId = response.Item2 });
    }

    [HttpPost("transcribe")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AudioChat([FromForm] TranscribeRequest request)
    {
        Stream fileStream = request.Audio.OpenReadStream();
        var audioClient = new AudioClient("gpt-4o-transcribe", _openAiApiKey);
        var audioOptions = new AudioTranscriptionOptions()
        {
            Language = "lt",
            ResponseFormat = AudioTranscriptionFormat.Text,
        };

        var response = await audioClient.TranscribeAudioAsync(fileStream, request.Audio.FileName, audioOptions);
        if (string.IsNullOrWhiteSpace(response.Value.Text))
        {
            _logger.Error("OpenAI didn't respond to transcribe request.");
            return StatusCode(500, "Error while processing OpenAI response. Try again later.");
        }

        _logger.Information($"OpenAI response for audio transcription: '{response.Value.Text}'");

        return Ok(new { message = response.Value.Text });
    }

    private enum ChatMessageType
    {
        System = 0,
        User = 1,
        Assistant = 2
    }

    private async Task<(string?, int)> GetAiResponseAsync(string aiModelName, string[] systemMessages, string prompt, int? contextId = null)
    {
        var nextIndex = 0;
        var messages = systemMessages
            .Select(message => new SystemChatMessage(message))
            .Cast<ChatMessage>()
            .ToList();

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

        messages.Add(new UserChatMessage(prompt));
        _dbContext.ChatHistory.Add(new ChatHistory
        {
            ContextId = (int)contextId,
            OrderIndex = nextIndex,
            MessageType = (int)ChatMessageType.User,
            Message = prompt
        });

        var openAiClient = new ChatClient(aiModelName,_openAiApiKey);
        var chatOptions = new ChatCompletionOptions
        {
            MaxOutputTokenCount = 2000
        };
        var response = await openAiClient.CompleteChatAsync(messages, chatOptions);

        var assistantResponse = response?.Value?.Content?[0]?.Text;
        if (!string.IsNullOrWhiteSpace(assistantResponse))
        {
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
