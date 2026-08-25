namespace Inventory.Application.DTOs;

public class UpdateProductRequest
{
    public string Code { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
}