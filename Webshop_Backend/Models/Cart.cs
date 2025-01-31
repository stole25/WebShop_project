namespace Webshop_Backend.Models;

public class Cart
{
    public int Id { get; set; }
    public List<CartItem> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class CartItem
{
    public int Id { get; set; } // Primary key
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
