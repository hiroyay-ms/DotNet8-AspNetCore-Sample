using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data;

public class AdventureWorksContext : DbContext
{
    private readonly HttpContext _httpContext;
    public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
        : base(options)
    {
        //_httpContext = httpContextAccessor?.HttpContext;
    }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var serverName = _httpContext?.Request.Headers["x-server-name"];
            optionsBuilder.UseSqlServer($"Server=tcp:{serverName}.database.windows.net,1433;Initial Catalog=AdventureWorksLT;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;");
        }
    }*/

    public DbSet<ProductCategory> ProductCategory => Set<ProductCategory>();
    public DbSet<ProductModel> ProductModel => Set<ProductModel>();
    public DbSet<ProductDescription> ProductDescription => Set<ProductDescription>();
    public DbSet<ProductModelProductDescription> ProductModelProductDescription => Set<ProductModelProductDescription>();
    public DbSet<Product> Product => Set<Product>();
    public DbSet<Address> Address => Set<Address>();
    public DbSet<Customer> Customer => Set<Customer>();
    public DbSet<SalesOrderHeader> SalesOrderHeader => Set<SalesOrderHeader>();
    public DbSet<SalesOrderDetail> SalesOrderDetail => Set<SalesOrderDetail>();

    public DbSet<Titles> Titles => Set<Titles>();

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
        modelBuilder.Entity<Titles>().ToTable("titles", "Pubs");
    }
}