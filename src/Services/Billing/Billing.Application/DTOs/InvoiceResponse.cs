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