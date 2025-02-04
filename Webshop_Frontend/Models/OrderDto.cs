using System.ComponentModel.DataAnnotations;

namespace Webshop_Frontend.Models;

public class OrderDto
{
    [Required(ErrorMessage = "Ime i prezime je obavezno")]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "Adresa je obavezna")]
    public string Address { get; set; }
    
    [Required(ErrorMessage = "Email je obavezan")]
    [EmailAddress(ErrorMessage = "Neispravan format email adrese")]
    public string Email { get; set; }
    
    [Phone(ErrorMessage = "Neispravan format telefonskog broja")]
    public string? Phone { get; set; }
    
    [Required(ErrorMessage = "Odaberite način plaćanja")]
    public PaymentMethod PaymentMethod { get; set; }
}