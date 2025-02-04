namespace Webshop_Frontend.Models;

public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

// Models/PaymentMethod.cs (frontend)
public enum PaymentMethod
{
    Gotovina,
    Kartica,
    PayPal
}