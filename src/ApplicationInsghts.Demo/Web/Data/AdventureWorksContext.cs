using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data;

public class AdventureWorksContext : DbContext
{
    public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
        : base(options)
    {
    }

    public DbSet<ProductCategory> ProductCategory => Set<ProductCategory>();
    public DbSet<Product> Product => Set<Product>();

    public DbSet<Titles> Titles => Set<Titles>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory", "SalesLT");
        modelBuilder.Entity<Product>().ToTable("Product", "SalesLT");
        modelBuilder.Entity<Titles>().ToTable("titles", "Pubs");
    }
}
