using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(
        DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
     protected override void OnModelCreating(
         ModelBuilder modelBuilder) 
         { 
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Product>(entity => 
            { 
                entity.ToTable("Products"); entity.HasKey(product => product.Id); 
                entity.Property(product => product.Description) .IsRequired() .HasMaxLength(200);
                 entity.Property(product => product.Price) .HasPrecision(18, 2); 
                 entity.Property(product => product.StockQuantity) .IsRequired(); }); 
        }
}