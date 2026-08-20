using Microsoft.EntityFrameworkCore;
using Inventory.Domain.Entities;
namespace Inventory.Infrastructure.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(
        DbContextOptions<InventoryDbContext> options) 
        : base(options)
    {
    }
     public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(InventoryDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
