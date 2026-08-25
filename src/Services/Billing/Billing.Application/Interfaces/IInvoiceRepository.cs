using Billing.Domain.Entities;

namespace Billing.Application.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<Invoice?> GetLastAsync(
        CancellationToken cancellationToken);

    Task AddAsync(
        Invoice invoice,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
    
    Task<string> GetNextInvoiceNumberAsync(
        CancellationToken cancellationToken);
}