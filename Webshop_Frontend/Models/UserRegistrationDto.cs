using System.ComponentModel.DataAnnotations;

namespace Webshop_Frontend.Models;

public class UserRegistrationDto
{
    [Required(ErrorMessage = "Email je obavezan")]
    [EmailAddress(ErrorMessage = "Neispravan format emaila")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Lozinka je obavezna")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati najmanje 6 znakova")]
    public string Password { get; set; }
}
