using System.ComponentModel.DataAnnotations;

namespace Models.Db;

public class ChatHistory
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ContextId { get; set; }

    [Required]
    public int OrderIndex { get; set; }

    [Required]
    public int MessageType { get; set; }

    [Required]
    public required string Message { get; set; }
}
