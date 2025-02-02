using System.ComponentModel.DataAnnotations;

namespace Webshop_Backend.DTOs;

// Webshop_Backend/DTOs/UserRegistrationDto.cs
public class UserRegistrationDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
}
