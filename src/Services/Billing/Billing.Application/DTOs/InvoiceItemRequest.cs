namespace Billing.Application.DTOs;

public class InvoiceItemRequest
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
