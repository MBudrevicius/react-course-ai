using System.ComponentModel.DataAnnotations;

namespace Models.Request;

public class ProblemRequest
{
    [Required]
    public required string Question { get; set; }
}

public class BestSubmissionRequest
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int ProblemId { get; set; }
}