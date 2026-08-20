using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Data;

public class BillingDbContext : DbContext
{
    public BillingDbContext(
        DbContextOptions<BillingDbContext> options) 
        : base(options)
    {
    }
}
