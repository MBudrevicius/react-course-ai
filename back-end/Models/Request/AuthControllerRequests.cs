using System.ComponentModel.DataAnnotations;

namespace Models.Request;

public class RegisterRequest
{
    [Required, StringLength(20, MinimumLength = 3)]
    public required string Username { get; set; }

    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required, MinLength(6)]
    public required string Password { get; set; }
}

public class LoginRequest
{
    [Required]
    public required string UsernameOrEmail { get; set; }

    [Required]
    public required string Password { get; set; }
}

