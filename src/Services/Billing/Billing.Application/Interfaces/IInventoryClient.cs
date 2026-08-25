namespace Billing.Application.Interfaces;

public interface IInventoryClient
{
    Task<bool> ProductExistsAsync(
        Guid productId,
        CancellationToken cancellationToken = default);

    Task<bool> RemoveStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default);

    Task<bool> AddStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default);

  
}