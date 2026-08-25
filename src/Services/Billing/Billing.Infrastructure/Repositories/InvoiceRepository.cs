using System.Data;
using Billing.Application.Interfaces;
using Billing.Domain.Entities;
using Billing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly BillingDbContext _context;

    public InvoiceRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Invoices
            .Include(invoice => invoice.Items)
            .FirstOrDefaultAsync(
                invoice => invoice.Id == id,
                cancellationToken);
    }

    public async Task<Invoice?> GetLastAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Invoices
            .Include(invoice => invoice.Items)
            .OrderByDescending(invoice => invoice.IssuedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(
        Invoice invoice,
        CancellationToken cancellationToken)
    {
        await _context.Invoices.AddAsync(
            invoice,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> GetNextInvoiceNumberAsync(
        CancellationToken cancellationToken)
    {
        await using var command =
            _context.Database.GetDbConnection().CreateCommand();

        command.CommandText =
            "SELECT NEXT VALUE FOR InvoiceNumberSequence";

        command.CommandType = CommandType.Text;

        if (command.Connection!.State != ConnectionState.Open)
        {
            await command.Connection.OpenAsync(cancellationToken);
        }

        var result =
            await command.ExecuteScalarAsync(cancellationToken);

        var number = Convert.ToInt64(result);

        return $"NF-{number:D6}";
    }
}