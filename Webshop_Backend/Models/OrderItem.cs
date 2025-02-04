namespace Webshop_Backend.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public enum PaymentMethod
{
    Gotovina,
    Kartica,
    PayPal
}