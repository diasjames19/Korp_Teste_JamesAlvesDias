namespace Billing.Infrastructure.Clients;

public class InventoryProblemDetails
{
    public string? Title { get; set; }

    public int Status { get; set; }

    public string? Detail { get; set; }

    public string? Instance { get; set; }
}