namespace Billing.Application.Models;

public record StockMovement(
    Guid ProductId,
    int Quantity);