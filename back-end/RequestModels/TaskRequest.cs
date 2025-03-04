using System.ComponentModel.DataAnnotations;

namespace RequestModels;

public class TaskRequest
{
    [Required]
    public required string Question { get; set; }
}