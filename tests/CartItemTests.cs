// CartItemTests.cs
using Xunit;
using StoreApp.Models;

public class CartItemTests
{
    // Sunny day
    [Fact]
    public void CartItem_ShouldBeCreated()
    {
        var item = new CartItem();
        Assert.NotNull(item);
    }

    [Fact]
    public void CartItem_ShouldSetProperties()
    {
        var item = new CartItem
        {
            ProductId = 1,
            BuyerId = 2,
            Quantity = 3,
            Price = 19.99
        };

        Assert.Equal(1, item.ProductId);
        Assert.Equal(2, item.BuyerId);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(19.99, item.Price);
    }

    [Fact]
    public void CartItem_Subtotal_ShouldCalculateCorrectly()
    {
        var item = new CartItem { Price = 20.00, Quantity = 3 };
        double subtotal = item.Price * item.Quantity;
        Assert.Equal(60.00, subtotal);
    }

    // Rainy day
    [Fact]
    public void CartItem_ZeroQuantity_ShouldGiveZeroSubtotal()
    {
        var item = new CartItem { Price = 20.00, Quantity = 0 };
        double subtotal = item.Price * item.Quantity;
        Assert.Equal(0, subtotal);
    }

    [Fact]
    public void CartItem_DefaultQuantity_ShouldBeZero()
    {
        var item = new CartItem();
        Assert.Equal(0, item.Quantity);
    }
}