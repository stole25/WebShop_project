using System.ComponentModel.DataAnnotations;

namespace Webshop_Frontend.Models;

public class UserRegistrationDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
}