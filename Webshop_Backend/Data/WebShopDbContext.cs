using Webshop_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Webshop_Backend.Data;
using Microsoft.EntityFrameworkCore;


public class WebShopDbContext : DbContext
{
    public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Always call base first

        // Seed default category
        modelBuilder.Entity<Category>()
            .HasData(new Category { Id = 1, Name = "Uncategorized" });

        // Set default CategoryId for Products
        modelBuilder.Entity<Product>()
            .Property(p => p.CategoryId)
            .HasDefaultValue(1);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId);
    }

public DbSet<Webshop_Backend.Models.Order> Order { get; set; } = default!;
}

