namespace Webshop_Backend.Models;

public class CartItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public Product Product { get; set; }
    public decimal Price { get; set; }
}