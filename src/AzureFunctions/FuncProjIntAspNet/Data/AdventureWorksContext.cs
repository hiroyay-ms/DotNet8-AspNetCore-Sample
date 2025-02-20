using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FuncProjIntAspNet.Models;

namespace FuncProjIntAspNet.Data;

public class AdventureWorksContext : DbContext
{
    private readonly HttpContext _httpContext;
    public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options, IHttpContextAccessor httpContextAccessor = null)
        : base(options)
    {
        _httpContext = httpContextAccessor?.HttpContext;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") ?? throw new InvalidOperationException("Connection string 'SQL_CONNECTION_STRING' not found.");

            var serverName = _httpContext?.Request.Headers["x-server-name"];
            if (string.IsNullOrEmpty(serverName))
                serverName = Environment.GetEnvironmentVariable("DEFAULT_SERVER_NAME") ?? "localhost";

            connectionString = string.Format(connectionString, serverName);
            optionsBuilder.UseSqlServer(connectionString);
        }
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