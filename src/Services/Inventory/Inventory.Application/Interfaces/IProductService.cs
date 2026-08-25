using Inventory.Application.DTOs;

namespace Inventory.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<ProductResponse?> UpdateAsync(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default);

          Task<ProductResponse?> AddStockAsync(
        Guid id,
        StockMovementRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse?> RemoveStockAsync(
        Guid id,
        StockMovementRequest request,
        CancellationToken cancellationToken = default);
}