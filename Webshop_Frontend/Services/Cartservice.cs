using Blazored.LocalStorage;
using Webshop_Frontend.Shared;

namespace Webshop_Frontend.Services
{
    public class CartService
    {
        private readonly ILocalStorageService _localStorage;
        public List<CartItem> Items { get; private set; } = new();
        public event Action OnChange;

        public CartService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task LoadCart()
        {
            Items = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
            NotifyStateChanged();
        }

        public async Task AddToCart(Product product)
        {
            var existingItem = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                Items.Add(new CartItem { Product = product, Quantity = 1 });
            }

            await SaveCart();
        }

        public async Task RemoveFromCart(int productId)
        {
            var item = Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                Items.Remove(item);
                await SaveCart();
            }
        }
        public void ClearCart()
        {
            Items.Clear();
        }

        public async Task UpdateQuantity(int productId, int newQuantity)
        {
            var item = Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                item.Quantity = newQuantity;
                await SaveCart();
            }
        }

        public decimal GetTotal() => Items.Sum(i => i.Product.Price * i.Quantity);

        private async Task SaveCart()
        {
            await _localStorage.SetItemAsync("cart", Items);
            NotifyStateChanged();
        }
        
        

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;
    }
    
}