using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id)
            .ValueGeneratedNever();

        builder.Property(product => product.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(product => product.Code)
            .IsUnique();

        builder.Property(product => product.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(product => product.StockQuantity)
            .IsRequired();
    }   
}
