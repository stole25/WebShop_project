using System.ComponentModel.DataAnnotations;

namespace Webshop_Backend.DTOs;

public class OrderDto
{
    [Required]
    public string FullName { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
    
    [Required]
    public PaymentMethod PaymentMethod { get; set; }
}

public enum PaymentMethod
{
    Gotovina,
    Kartica,
    PayPal
}