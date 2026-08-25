using Billing.Domain.Enums;

namespace Billing.Domain.Entities;

public class Invoice
{
    private readonly List<InvoiceItem> _items = new();

    public Guid Id { get; private set; }

    public string Number { get; private set; } = string.Empty;

    public DateTime IssuedAt { get; private set; }

    public InvoiceStatus Status { get; private set; }

    public decimal TotalAmount =>
        _items.Sum(item => item.TotalPrice);

    public IReadOnlyCollection<InvoiceItem> Items =>
        _items.AsReadOnly();

    private Invoice()
    {
        // Required by EF Core.
    }

    public Invoice(
        string number,
        DateTime issuedAt)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException(
                "Invoice number is required.",
                nameof(number));
        }

        Id = Guid.NewGuid();
        Number = number.Trim();
        IssuedAt = issuedAt;
        Status = InvoiceStatus.Open;
    }

    public void AddItem(
        Guid productId,
        int quantity,
        decimal unitPrice)
    {
        EnsureOpen();

        var item = new InvoiceItem(
            productId,
            quantity,
            unitPrice);

        _items.Add(item);
    }

    public void Print()
    {
        EnsureOpen();

        if (_items.Count == 0)
        {
            throw new InvalidOperationException(
                "An invoice must contain at least one item.");
        }

        Status = InvoiceStatus.Closed;
    }

    private void EnsureOpen()
    {
        if (Status != InvoiceStatus.Open)
        {
            throw new InvalidOperationException(
                "Invoice is already closed.");
        }
    }
}