using Inventory.Domain.Entities;
using Inventory.Domain.Exceptions;

namespace Inventory.UnitTests;

public class ProductTests
{
   [Fact]
    public void Should_Create_Product_With_Valid_Data()
    {
        // Arrange
        var code = "P001";
        var description = "Notebook";
        var stockQuantity = 10;

        // Act
        var product = new Product(
            code,
            description,
            stockQuantity);

        // Assert
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal(code, product.Code);
        Assert.Equal(description, product.Description);
        Assert.Equal(stockQuantity, product.StockQuantity);
    }

    [Fact]
    public void Should_Not_Create_Product_With_Empty_Code()
    {
        // Arrange
        var code = "";
        var description = "Notebook";
        var stockQuantity = 10;

        // Act
        var action = () => new Product(
            code,
            description,
            stockQuantity);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Should_Not_Create_Product_With_Empty_Description()
    {
        // Arrange
        var code = "P001";
        var description = "";
        var stockQuantity = 10;

        // Act
        var action = () => new Product(
            code,
            description,
            stockQuantity);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Should_Not_Create_Product_With_Negative_Stock()
    {
        // Arrange
        var code = "P001";
        var description = "Notebook";
        var stockQuantity = -1;

        // Act
        var action = () => new Product(
            code,
            description,
            stockQuantity);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Should_Add_Stock()
    {
        // Arrange
        var product = new Product(
            "P001",
            "Notebook",
            10);

        // Act
        product.AddStock(5);

        // Assert
        Assert.Equal(15, product.StockQuantity);
    }

    [Fact]
    public void Should_Remove_Stock()
    {
        // Arrange
        var product = new Product(
            "P001",
            "Notebook",
            10);

        // Act
        product.RemoveStock(3);

        // Assert
        Assert.Equal(7, product.StockQuantity);
    }

    [Fact]
    public void Should_Not_Remove_More_Stock_Than_Available()
    {
        // Arrange
        var product = new Product(
            "P001",
            "Notebook",
            10);

        // Act
        var action = () => product.RemoveStock(11);

        // Assert
        Assert.Throws<InsufficientStockException>(action);
    }

    [Fact]
    public void Should_Not_Add_Zero_Or_Negative_Quantity()
    {
        // Arrange
        var product = new Product(
            "P001",
            "Notebook",
            10);

        // Act
        var action = () => product.AddStock(0);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Should_Not_Remove_Zero_Or_Negative_Quantity()
    {
        // Arrange
        var product = new Product(
            "P001",
            "Notebook",
            10);

        // Act
        var action = () => product.RemoveStock(0);

        // Assert
        Assert.Throws<ArgumentException>(action);
    } 
}
