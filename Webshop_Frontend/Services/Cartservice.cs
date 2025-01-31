using Webshop_Frontend.Shared;

namespace Webshop_Frontend.Services;

public class CartService
{
    public List<Product> Items { get; private set; } = new();
    public event Action OnChange;

    public void AddToCart(Product product)
    {
        var existingItem = Items.FirstOrDefault(i => i.Id == product.Id);
        if (existingItem == null)
        {
            Items.Add(product);
        }
        NotifyStateChanged();
    }

    public void RemoveFromCart(int productId)
    {
        var item = Items.FirstOrDefault(i => i.Id == productId);
        if (item != null)
        {
            Items.Remove(item);
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
