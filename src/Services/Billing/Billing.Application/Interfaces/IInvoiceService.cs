using Billing.Application.DTOs;

namespace Billing.Application.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceResponse> CreateAsync(
        CreateInvoiceRequest request,
        CancellationToken cancellationToken = default);

    Task<InvoiceResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<InvoiceResponse?> PrintAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}