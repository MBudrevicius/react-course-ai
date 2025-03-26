using System.ComponentModel.DataAnnotations;

namespace Models.Request;

public class ProblemRequest
{
    [Required]
    public required string Question { get; set; }
}