using System.ComponentModel.DataAnnotations;

namespace Webshop_Backend.DTOs;

// Webshop_Backend/DTOs/UserLoginDto.cs
public class UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
