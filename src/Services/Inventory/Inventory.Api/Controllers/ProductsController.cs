using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(
        IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(
            id,
            cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Update(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.UpdateAsync(
            id,
            request,
            cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost("{id:guid}/stock/in")]
    public async Task<ActionResult<ProductResponse>> AddStock(
        Guid id,
        [FromBody] StockMovementRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.AddStockAsync(
            id,
            request,
            cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

   [HttpPost("{id:guid}/stock/out")]
public async Task<ActionResult<ProductResponse>> RemoveStock(
    Guid id,
    [FromBody] StockMovementRequest request,
    CancellationToken cancellationToken)
{
    var product = await _productService.RemoveStockAsync(
        id,
        request,
        cancellationToken);

    if (product is null)
    {
        return NotFound();
    }

    return Ok(product);
}
}