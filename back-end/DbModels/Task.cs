using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbModels;

public class Task
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Lesson")]
    public int LessonId { get; set; }

    [Required]
    public int OrderIndex { get; set; }

    [Required]
    public required string Question { get; set; }
}
