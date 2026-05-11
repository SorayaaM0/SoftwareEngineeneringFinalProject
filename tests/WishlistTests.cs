// WishlistTests.cs
using Xunit;
using StoreApp.Models;

public class WishlistTests
{
    // Sunny day
    [Fact]
    public void Wishlist_ShouldBeCreated()
    {
        var wishlist = new Wishlist();
        Assert.NotNull(wishlist);
    }

    [Fact]
    public void Wishlist_ShouldSetBuyerAndProduct()
    {
        var wishlist = new Wishlist
        {
            BuyerId = 1,
            ProductId = 5
        };
        Assert.Equal(1, wishlist.BuyerId);
        Assert.Equal(5, wishlist.ProductId);
    }

    // Rainy day
    [Fact]
    public void Wishlist_DefaultIds_ShouldBeZero()
    {
        var wishlist = new Wishlist();
        Assert.Equal(0, wishlist.BuyerId);
        Assert.Equal(0, wishlist.ProductId);
    }
}