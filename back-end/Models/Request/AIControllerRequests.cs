using System.ComponentModel.DataAnnotations;

namespace Models.Request;

public class CodeSubmissionRequest
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int ProblemId { get; set; }

    [Required]
    public required string CodeSubmission { get; set; }
}

public class ChatRequest
{
    [Required]
    public required string Message { get; set; }
}
