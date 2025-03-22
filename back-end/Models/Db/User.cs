
using System.ComponentModel.DataAnnotations;

namespace Models.Db;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(20, MinimumLength = 3)]
    public required string Username { get; set; }

    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string PasswordHash { get; set; }
}