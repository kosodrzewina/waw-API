using System.ComponentModel.DataAnnotations;

namespace WawAPI.DTOs;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Password { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string RetypedPassword { get; set; } = default!;
}
