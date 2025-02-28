using System.ComponentModel.DataAnnotations;

namespace RequestModels;

public class LoginRequest
{
    [Required]
    public required string UsernameOrEmail { get; set; }

    [Required]
    public required string Password { get; set; }
}

