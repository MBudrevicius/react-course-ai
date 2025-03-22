using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Db;

public class CodeSubmission
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    [ForeignKey("Problem")]
    public int ProblemId { get; set; }

    [Required]
    public required string Code { get; set; }

    [Required]
    public int Score { get; set; }

    [Required]
    public required string Feedback { get; set; }
}
