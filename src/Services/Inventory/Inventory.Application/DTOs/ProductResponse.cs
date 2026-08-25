namespace Inventory.Application.DTOs;

public class ProductResponse
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int StockQuantity { get; set; }

    public decimal Price { get; set; }
}