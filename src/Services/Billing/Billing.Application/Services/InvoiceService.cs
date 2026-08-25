using Billing.Application.DTOs;
using Billing.Application.Interfaces;
using Billing.Application.Models;
using Billing.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IInventoryClient _inventoryClient;
    private readonly ILogger<InvoiceService> _logger;

    public InvoiceService(
        IInvoiceRepository invoiceRepository,
        IInventoryClient inventoryClient,
        ILogger<InvoiceService> logger)
    {
        _invoiceRepository = invoiceRepository;
        _inventoryClient = inventoryClient;
        _logger = logger;
    }

    public async Task<InvoiceResponse> CreateAsync(
        CreateInvoiceRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        await ValidateProductsExistAsync(
            request,
            cancellationToken);

        var stockMovements = new List<StockMovement>();

        try
        {
            await RemoveStockAsync(
                request,
                stockMovements,
                cancellationToken);

            var invoice = await CreateInvoiceAsync(
                request,
                cancellationToken);

            return MapToResponse(invoice);
        }
        catch
        {
            await CompensateStockAsync(stockMovements);

            throw;
        }
    }

    private static void ValidateRequest(
        CreateInvoiceRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(
                nameof(request));
        }

        if (request.Items is null ||
            request.Items.Count == 0)
        {
            throw new ArgumentException(
                "Invoice must contain at least one item.",
                nameof(request));
        }
    }

    private async Task ValidateProductsExistAsync(
        CreateInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var productExists =
                await _inventoryClient.ProductExistsAsync(
                    item.ProductId,
                    cancellationToken);

            if (!productExists)
            {
                throw new KeyNotFoundException(
                    $"Product '{item.ProductId}' was not found.");
            }
        }
    }

    private async Task RemoveStockAsync(
        CreateInvoiceRequest request,
        List<StockMovement> stockMovements,
        CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            await _inventoryClient.RemoveStockAsync(
                item.ProductId,
                item.Quantity,
                cancellationToken);

            stockMovements.Add(
                new StockMovement(
                    item.ProductId,
                    item.Quantity));
        }
    }

    private async Task<Invoice> CreateInvoiceAsync(
        CreateInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        var invoiceNumber =
            await _invoiceRepository.GetNextInvoiceNumberAsync(
                cancellationToken);

        var invoice = new Invoice(
            invoiceNumber,
            DateTime.UtcNow);

        foreach (var item in request.Items)
        {
            invoice.AddItem(
                item.ProductId,
                item.Quantity,
                item.UnitPrice);
        }

        await _invoiceRepository.AddAsync(
            invoice,
            cancellationToken);

        await _invoiceRepository.SaveChangesAsync(
            cancellationToken);

        return invoice;
    }

    private async Task CompensateStockAsync(
        List<StockMovement> stockMovements)
    {
        foreach (var movement in stockMovements.AsEnumerable().Reverse())
        {
            try
            {
                var compensated =
                    await _inventoryClient.AddStockAsync(
                        movement.ProductId,
                        movement.Quantity,
                        CancellationToken.None);

                if (!compensated)
                {
                    _logger.LogError(
                        "Stock compensation returned false for product {ProductId}. Quantity: {Quantity}",
                        movement.ProductId,
                        movement.Quantity);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to compensate stock for product {ProductId}. Quantity: {Quantity}",
                    movement.ProductId,
                    movement.Quantity);
            }
        }
    }

    public async Task<InvoiceResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var invoice =
            await _invoiceRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (invoice is null)
        {
            return null;
        }

        return MapToResponse(invoice);
    }

    public async Task<InvoiceResponse?> PrintAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var invoice =
            await _invoiceRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (invoice is null)
        {
            return null;
        }

        invoice.Print();

        await _invoiceRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(invoice);
    }

    private static InvoiceResponse MapToResponse(
        Invoice invoice)
    {
        return new InvoiceResponse
        {
            Id = invoice.Id,
            Number = invoice.Number,
            Status = invoice.Status,
            IssuedAt = invoice.IssuedAt,
            TotalAmount = invoice.TotalAmount,

            Items = invoice.Items
                .Select(item => new InvoiceItemResponse
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                })
                .ToList()
        };
    }
}