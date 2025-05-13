using Microsoft.EntityFrameworkCore;
using FuncCustomClaimProvider.Models;

namespace FuncCustomClaimProvider.Data;

public class AdventureWorksContext : DbContext
{
    public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
        : base(options)
    {
    }

    public DbSet<Customers> Customers => Set<Customers>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customers>().ToTable("Customers", "dbo");
    }
}