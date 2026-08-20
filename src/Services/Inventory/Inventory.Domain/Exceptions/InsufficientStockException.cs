namespace Inventory.Domain.Exceptions;

public class InsufficientStockException :Exception
{
     public InsufficientStockException(
        string productCode,
        int requestedQuantity,
        int availableQuantity)
        : base(
            $"Insufficient stock for product '{productCode}'. " +
            $"Requested: {requestedQuantity}. " +
            $"Available: {availableQuantity}.")
    {
        ProductCode = productCode;
        RequestedQuantity = requestedQuantity;
        AvailableQuantity = availableQuantity;
    }

    public string ProductCode { get; }

    public int RequestedQuantity { get; }

    public int AvailableQuantity { get; }
}
