
using Billing.Domain.Enums;

namespace Billing.Application.DTOs;

public class InvoiceResponse
{
    public Guid Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public InvoiceStatus Status { get; set; }

    public DateTime IssuedAt { get; set; }

    public decimal TotalAmount { get; set; }

    public List<InvoiceItemResponse> Items { get; set; } = [];
}

public class InvoiceItemResponse
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}