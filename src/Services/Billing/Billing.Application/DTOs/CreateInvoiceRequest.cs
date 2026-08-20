

namespace Billing.Application.DTOs;

public class CreateInvoiceRequest
{
      public List<InvoiceItemRequest> Items { get; set; } = [];
}
