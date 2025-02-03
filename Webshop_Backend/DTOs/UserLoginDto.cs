using System.ComponentModel.DataAnnotations;

namespace Webshop_Backend.DTOs;

// Webshop_Backend/DTOs/UserLoginDto.cs
public class UserLoginDto
{
    [Required(ErrorMessage = "Email je obavezan!")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Lozinka je obavezna!")]
    [StringLength(100, ErrorMessage = "Lozinka mora imati najmanje {6} znakova.", MinimumLength = 6)]
    public string Password { get; set; }
}


