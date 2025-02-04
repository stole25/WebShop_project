using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop_Backend.Data;
using Webshop_Backend.Models;

namespace Webshop_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly WebShopDbContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(WebShopDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.Items.Count == 0)
                return BadRequest("Košarica je prazna");

            var order = new Order
            {
                UserId = userId,
                FullName = orderDto.FullName,
                Address = orderDto.Address,
                Email = orderDto.Email,
                Phone = orderDto.Phone,
                PaymentMethod = orderDto.PaymentMethod,
                Total = cart.Items.Sum(i => i.Quantity * i.Price),
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            cart.Items.Clear();
            await _context.SaveChangesAsync();

            return Ok(new { OrderId = order.Id });
        }
    }

    public class OrderDto
    {
        [Required]
        public string FullName { get; set; }
    
        [Required]
        public string Address { get; set; }
    
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    
        [Phone]
        public string? Phone { get; set; }
    
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
    }

}
