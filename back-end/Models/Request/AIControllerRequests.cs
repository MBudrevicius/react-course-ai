using System.ComponentModel.DataAnnotations;

namespace Models.Request;

public class EvaluateCodeRequest
{
    [Required]
    public required string CodeSubmission { get; set; }
}

public class ChatRequest
{
    [Required]
    public required string Message { get; set; }

    public int? ContextId { get; set; }
}
