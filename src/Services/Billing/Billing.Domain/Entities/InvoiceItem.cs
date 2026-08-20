namespace Billing.Domain.Entities;

public class InvoiceItem
{
    public Guid Id { get; private set; }

    public Guid InvoiceId { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal TotalPrice => Quantity * UnitPrice;

    private InvoiceItem()
    {
        // Required by EF Core.
    }

    public InvoiceItem(
        Guid productId,
        int quantity,
        decimal unitPrice)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException(
                "Product is required.",
                nameof(productId));
        }

        if (quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.",
                nameof(quantity));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException(
                "Unit price cannot be negative.",
                nameof(unitPrice));
        }

        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    } 
}
