using System.ComponentModel.DataAnnotations;

namespace DbModels;

public class Lesson
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(50)]
    public required string Title { get; set; }

    [Required]
    public int OrderIndex { get; set; }

    [Required]
    public required string Content { get; set; }
}
