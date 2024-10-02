using Microsoft.EntityFrameworkCore;
using Aspnet.DataEntities;

namespace Aspnet.Products.Api.Data;

public class AdventureWorksContext : DbContext
{
    public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductDescription> ProductDescriptions => Set<ProductDescription>();
    public DbSet<ProductModel> ProductModels => Set<ProductModel>();
    public DbSet<ProductModelProductDescription> ProductModelProductDescriptions => Set<ProductModelProductDescription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Product", schema: "SalesLT");
        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory", schema: "SalesLT");
        modelBuilder.Entity<ProductDescription>().ToTable("ProductDescription", schema: "SalesLT");
        modelBuilder.Entity<ProductModel>().ToTable("ProductModel", schema: "SalesLT");
        modelBuilder.Entity<ProductModelProductDescription>().ToTable("ProductModelProductDescription", schema: "SalesLT").HasKey(p => new { p.ProductModelId, p.ProductDescriptionId, p.Culture });
    }
}