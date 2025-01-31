using Webshop_Backend.Models;

namespace Webshop_Backend.Data;
using Microsoft.EntityFrameworkCore;


public class WebShopDbContext : DbContext
{
    public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
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
    }
}

