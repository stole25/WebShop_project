namespace Webshop_Frontend.Models;

public class OrderDto
{
    public string FullName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}