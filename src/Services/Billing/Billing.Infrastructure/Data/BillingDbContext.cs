using Microsoft.EntityFrameworkCore;
using Billing.Domain.Entities;

namespace Billing.Infrastructure.Data;

public class BillingDbContext : DbContext
{
    public BillingDbContext(
        DbContextOptions<BillingDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BillingDbContext).Assembly);
    }
}
