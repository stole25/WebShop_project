using Webshop_Backend.Models;

namespace Webshop_Backend.Data;
using Microsoft.EntityFrameworkCore;


public class WebShopDbContext : DbContext
{
    public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
}