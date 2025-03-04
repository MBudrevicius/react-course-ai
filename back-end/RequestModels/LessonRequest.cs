using System.ComponentModel.DataAnnotations;

namespace RequestModels;

public class LessonRequest
{

    [Required, StringLength(50)]
    public required string Title { get; set; }

    [Required]
    public required string Content { get; set; }
}
