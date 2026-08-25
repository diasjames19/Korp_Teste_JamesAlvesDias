using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Code { get; private set; }

    public string Description { get; private set; }

    public int StockQuantity { get; private set; }

    public decimal Price { get; private set; }

    private Product()
    {
        Code = string.Empty;
        Description = string.Empty;
    }

    public Product(
        string code,
        string description,
        int stockQuantity,
        decimal price)
    {
        Id = Guid.NewGuid();

        SetCode(code);
        SetDescription(description);
        SetStockQuantity(stockQuantity);
        SetPrice(price);
    }

    public void Update(
        string code,
        string description,
        decimal price)
    {
        SetCode(code);
        SetDescription(description);
        SetPrice(price);
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

    private void SetPrice(decimal price)
{
    if (price < 0)
    {
        throw new ArgumentException(
            "Price cannot be negative.",
            nameof(price));
    }

    Price = price;
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