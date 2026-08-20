
using Inventory.Domain.Exceptions;
namespace Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Code { get; private set; }

    public string Description { get; private set; }

    public int StockQuantity { get; private set; }

    private Product()
    {
        // Required by EF Core.
    }

    public Product(
        string code,
        string description,
        int stockQuantity)
    {
        Id = Guid.NewGuid();

        SetCode(code);
        SetDescription(description);
        SetStockQuantity(stockQuantity);
    }

    public void Update(
        string code,
        string description)
    {
        SetCode(code);
        SetDescription(description);
    }

    public void AddStock(int quantity)
    {
        ValidatePositiveQuantity(quantity);

        StockQuantity += quantity;
    }

    public void RemoveStock(int quantity)
    {
        ValidatePositiveQuantity(quantity);

        if (quantity > StockQuantity)
        {
            throw new InsufficientStockException(
                    Code,
                    quantity,
                    StockQuantity);
        }

        StockQuantity -= quantity;
    }

    private void SetCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(
                "Product code is required.",
                nameof(code));
        }

        Code = code.Trim();
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Product description is required.",
                nameof(description));
        }

        Description = description.Trim();
    }

    private void SetStockQuantity(int stockQuantity)
    {
        if (stockQuantity < 0)
        {
            throw new ArgumentException(
                "Stock quantity cannot be negative.",
                nameof(stockQuantity));
        }

        StockQuantity = stockQuantity;
    }

    private static void ValidatePositiveQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.",
                nameof(quantity));
        }
    }
}