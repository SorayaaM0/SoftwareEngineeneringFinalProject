// BuyerTests.cs
using Xunit;
using StoreApp.Models;

public class BuyerTests
{
    // Sunny day
    [Fact]
    public void Buyer_ShouldBeCreated()
    {
        var buyer = new Buyer();
        Assert.NotNull(buyer);
    }

    [Fact]
    public void Buyer_ShouldHaveCorrectUserType()
    {
        var buyer = new Buyer { UserType = "Buyer" };
        Assert.Equal("Buyer", buyer.UserType);
    }

    [Fact]
    public void Buyer_ShippingAddress_ShouldBeSettable()
    {
        var buyer = new Buyer { ShippingAddress = "123 Main St" };
        Assert.Equal("123 Main St", buyer.ShippingAddress);
    }

    // Rainy day
    [Fact]
    public void Buyer_DefaultShippingAddress_ShouldBeNull()
    {
        var buyer = new Buyer();
        Assert.Null(buyer.ShippingAddress);
    }

    [Fact]
    public void Buyer_ShouldNotBeAdmin()
    {
        var buyer = new Buyer { UserType = "Buyer" };
        Assert.NotEqual("Admin", buyer.UserType);
    }
}