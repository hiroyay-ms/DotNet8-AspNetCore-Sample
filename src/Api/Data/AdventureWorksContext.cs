using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data;

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
    public DbSet<Address> Address => Set<Address>();
    public DbSet<Customer> Customer => Set<Customer>();
    public DbSet<SalesOrderHeader> SalesOrderHeader => Set<SalesOrderHeader>();
    public DbSet<SalesOrderDetail> SalesOrderDetail => Set<SalesOrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory", "SalesLT");
        modelBuilder.Entity<ProductModel>().ToTable("ProductModel", "SalesLT");
        modelBuilder.Entity<ProductDescription>().ToTable("ProductDescription", "SalesLT");
        modelBuilder.Entity<ProductModelProductDescription>().ToTable("ProductModelProductDescription", "SalesLT").HasKey(p => new { p.ProductModelID, p.ProductDescriptionID });
        modelBuilder.Entity<Product>().ToTable("Product", "SalesLT");
        modelBuilder.Entity<Address>().ToTable("Address", "SalesLT");
        modelBuilder.Entity<Customer>().ToTable("Customer", "SalesLT");
        modelBuilder.Entity<SalesOrderHeader>().ToTable("SalesOrderHeader", "SalesLT");
        modelBuilder.Entity<SalesOrderDetail>().ToTable("SalesOrderDetail", "SalesLT");
    }
}