using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop_Backend.Data;
using Webshop_Backend.Models;

namespace Webshop_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly WebShopDbContext _context;

        public CartController(WebShopDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            return cart ?? await CreateNewCart(userId);
        }

        [HttpPost("AddItem")]
        public async Task<ActionResult> AddCartItem([FromBody] CartItemRequest request)
        {
            // Dohvati ili kreiraj košaricu
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            if (cart == null)
            {
                cart = new Cart { UserId = request.UserId };
                _context.Carts.Add(cart);
            }

            // Provjeri proizvod
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return NotFound("Proizvod nije pronađen");
            }

            // Ažuriraj košaricu
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    Price = product.Price
                });
            }

            await _context.SaveChangesAsync();
            return Ok(cart);
        }

        private async Task<Cart> CreateNewCart(int userId)
        {
            var newCart = new Cart { UserId = userId };
            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();
            return newCart;
        }

        public class CartItemRequest
        {
            public int UserId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; } = 1;
        }
    }
}
