using Microsoft.EntityFrameworkCore;
using FuncProj.Models;

namespace FuncProj.Data;

public class AdventureWorksContext : DbContext
{
    public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
        : base(options)
    {
    }

    public DbSet<ProductCategory> ProductCategory => Set<ProductCategory>();
    public DbSet<ProductModel> ProductModel => Set<ProductModel>();
    public DbSet<ProductDescription> ProductDescription => Set<ProductDescription>();
    public DbSet<ProductModelProductDescription> ProductModelProductDescription => Set<ProductModelProductDescription>();
    public DbSet<Product> Product => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory", "SalesLT");
        modelBuilder.Entity<ProductModel>().ToTable("ProductModel", "SalesLT");
        modelBuilder.Entity<ProductDescription>().ToTable("ProductDescription", "SalesLT");
        modelBuilder.Entity<ProductModelProductDescription>().ToTable("ProductModelProductDescription", "SalesLT").HasKey(p => new { p.ProductModelID, p.ProductDescriptionID });
        modelBuilder.Entity<Product>().ToTable("Product", "SalesLT");
    }
}
