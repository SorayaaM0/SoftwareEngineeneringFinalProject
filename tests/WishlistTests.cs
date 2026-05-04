using Xunit;
using StoreApp.Models;
public class WishlistTests
{
    [Fact]
    public void AddProduct_ShouldAddToWishlist()
    {
        var buyer = new Buyer(1, "Test User", "test@test.com", "password", "123 Ship St", "123 Bill St");
        var wishlist = new Wishlist(1, buyer);
        var product = new Product(1, "Phone", "Smart phone", 500, "", "Electronics");

        wishlist.addProduct(product);

        Assert.Single(wishlist.products);
    }

    [Fact]
    public void RemoveProduct_ShouldRemoveFromWishlist()
    {
        var buyer = new Buyer(1, "Test User", "test@test.com", "password", "123 Ship St", "123 Bill St");
        var wishlist = new Wishlist(1, buyer);
        var product = new Product(1, "Phone", "Smart phone", 500, "", "Electronics");

        wishlist.addProduct(product);
        wishlist.removeProduct(product);

        Assert.Empty(wishlist.products);
    }
}
