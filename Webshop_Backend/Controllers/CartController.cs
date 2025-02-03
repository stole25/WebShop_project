using System.Net.Http.Json;
using Webshop_Backend.Models;

namespace Webshop_Frontend.Services
{
    public class CartService
    {
        private readonly HttpClient _http;
        private Cart _cart = new();
        public event Action OnCartUpdated;

        public CartService(HttpClient http)
        {
            _http = http;
        }

        public List<CartItem> Items => _cart.Items;

        public async Task AddToCart(Product product, int quantity = 1)
        {
            var existingItem = _cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    Price = product.Price,
                    Product = product
                });
            }

            await SaveCart();
            OnCartUpdated?.Invoke();
        }

        public async Task LoadCart()
        {
            var userId = 1; // Privremeno hardkodirano
            _cart = await _http.GetFromJsonAsync<Cart>($"/api/cart/{userId}") ?? new Cart { UserId = userId };
            OnCartUpdated?.Invoke();
        }

        public async Task RemoveFromCart(int productId)
        {
            var item = _cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _cart.Items.Remove(item);
                await SaveCart();
                OnCartUpdated?.Invoke();
            }
        }

        public decimal GetTotal()
        {
            return _cart.Items.Sum(i => i.Quantity * i.Price);
        }

        public int GetItemCount()
        {
            return _cart.Items.Sum(i => i.Quantity);
        }

        private async Task SaveCart()
        {
            await _http.PutAsJsonAsync($"/api/cart/{_cart.UserId}", _cart);
        }
    }
}
