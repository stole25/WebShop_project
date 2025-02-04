namespace Webshop_Backend.Models;

public class Order
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public int UserId { get; set; }
}