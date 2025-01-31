namespace Webshop_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class WebShopDbContextFactory : IDesignTimeDbContextFactory<WebShopDbContext>
{
    public WebShopDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebShopDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=WebShopDB;User Id=sa;Password=Odfakujte2209;TrustServerCertificate=True;");
        
        return new WebShopDbContext(optionsBuilder.Options);
    }
}
