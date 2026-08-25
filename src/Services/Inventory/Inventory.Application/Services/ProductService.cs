using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = new Product(
            request.Code,
            request.Description,
            request.StockQuantity,
            request.Price);

        await _productRepository.AddAsync(
            product,
            cancellationToken);

        await _productRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(product);
    }

    public async Task<ProductResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (product is null)
        {
            return null;
        }

        return MapToResponse(product);
    }

    public async Task<ProductResponse?> UpdateAsync(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (product is null)
        {
            return null;
        }

        product.Update(
            request.Code,
            request.Description,
            request.Price);

        await _productRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(product);
    }

    
    private static ProductResponse MapToResponse(
        Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Code = product.Code,
            Description = product.Description,
            StockQuantity = product.StockQuantity,
            Price = product.Price
        };
    }

    public async Task<ProductResponse?> AddStockAsync(
    Guid id,
    StockMovementRequest request,
    CancellationToken cancellationToken = default)
{
    var product = await _productRepository.GetByIdAsync(
        id,
        cancellationToken);

    if (product is null)
    {
        return null;
    }

    product.AddStock(request.Quantity);

    await _productRepository.SaveChangesAsync(
        cancellationToken);

    return MapToResponse(product);
}

public async Task<ProductResponse?> RemoveStockAsync(
    Guid id,
    StockMovementRequest request,
    CancellationToken cancellationToken = default)
{
    var product = await _productRepository.GetByIdAsync(
        id,
        cancellationToken);

    if (product is null)
    {
        return null;
    }

    product.RemoveStock(request.Quantity);

    await _productRepository.SaveChangesAsync(
        cancellationToken);

    return MapToResponse(product);
 }
}