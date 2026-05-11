// OrderTests.cs — new file
using Xunit;
using StoreApp.Models;

public class OrderTests
{
    // Sunny day
    [Fact]
    public void Order_ShouldBeCreated()
    {
        var order = new Order();
        Assert.NotNull(order);
    }

    [Fact]
    public void Order_ShouldSetProperties()
    {
        var order = new Order
        {
            BuyerId = 1,
            TotalAmount = 99.99,
            Status = "Placed",
            OrderDate = DateTime.Now
        };

        Assert.Equal(1, order.BuyerId);
        Assert.Equal(99.99, order.TotalAmount);
        Assert.Equal("Placed", order.Status);
    }

    // Rainy day
    [Fact]
    public void Order_DefaultStatus_ShouldBeNull()
    {
        var order = new Order();
        Assert.Null(order.Status);
    }

    [Fact]
    public void Order_ZeroTotal_ShouldBeValid()
    {
        var order = new Order { TotalAmount = 0 };
        Assert.Equal(0, order.TotalAmount);
    }
}