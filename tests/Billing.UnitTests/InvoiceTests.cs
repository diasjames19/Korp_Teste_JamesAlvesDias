using Billing.Domain.Enums;
using Billing.Domain.Entities;
namespace Billing.UnitTests;

public class InvoiceTests
{
  [Fact]
    public void Should_Create_Invoice_As_Open()
    {
        // Arrange & Act
        var invoice = new Invoice(
            "NF-001",
            DateTime.UtcNow);

        // Assert
        Assert.Equal(
            InvoiceStatus.Open,
            invoice.Status);

        Assert.Empty(invoice.Items);

        Assert.Equal(
            0m,
            invoice.TotalAmount);
    }

    [Fact]
    public void Should_Add_Item_To_Invoice()
    {
        // Arrange
        var invoice = new Invoice(
            "NF-001",
            DateTime.UtcNow);

        var productId = Guid.NewGuid();

        // Act
        invoice.AddItem(
            productId,
            2,
            10.50m);

        // Assert
        var item = Assert.Single(invoice.Items);

        Assert.Equal(productId, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(10.50m, item.UnitPrice);
        Assert.Equal(21.00m, item.TotalPrice);

        Assert.Equal(
            21.00m,
            invoice.TotalAmount);
    }

    [Fact]
    public void Should_Print_Invoice_And_Close_It()
    {
        // Arrange
        var invoice = new Invoice(
            "NF-001",
            DateTime.UtcNow);

        invoice.AddItem(
            Guid.NewGuid(),
            2,
            10.50m);

        // Act
        invoice.Print();

        // Assert
        Assert.Equal(
            InvoiceStatus.Closed,
            invoice.Status);
    }

    [Fact]
    public void Should_Not_Print_Empty_Invoice()
    {
        // Arrange
        var invoice = new Invoice(
            "NF-001",
            DateTime.UtcNow);

        // Act
        var action = () => invoice.Print();

        // Assert
        Assert.Throws<InvalidOperationException>(
            action);
    }

    [Fact]
    public void Should_Not_Print_Closed_Invoice()
    {
        // Arrange
        var invoice = new Invoice(
            "NF-001",
            DateTime.UtcNow);

        invoice.AddItem(
            Guid.NewGuid(),
            1,
            100m);

        invoice.Print();

        // Act
        var action = () => invoice.Print();

        // Assert
        Assert.Throws<InvalidOperationException>(
            action);
    }

    [Fact]
    public void Should_Not_Add_Item_To_Closed_Invoice()
    {
        // Arrange
        var invoice = new Invoice(
            "NF-001",
            DateTime.UtcNow);

        invoice.AddItem(
            Guid.NewGuid(),
            1,
            100m);

        invoice.Print();

        // Act
        var action = () =>
            invoice.AddItem(
                Guid.NewGuid(),
                1,
                50m);

        // Assert
        Assert.Throws<InvalidOperationException>(
            action);
    }  
}
