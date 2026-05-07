using Xunit;
using StoreApp.Models;
public class WishlistTests
{
    [Fact]
    public void AddProduct_ShouldAddToWishlist()
    {
        var buyer = new Buyer();
        var wishlist = new Wishlist();
        var product = new Product();

        wishlist.addProduct(product.ProductId);

        //Assert.Single(wishlist.ProductId);
    }

    [Fact]
    public void RemoveProduct_ShouldRemoveFromWishlist()
    {
        var buyer = new Buyer();
        var wishlist = new Wishlist();
        var product = new Product();

        wishlist.addProduct(product.ProductId);
        wishlist.removeProduct(product.ProductId);

        //Assert.Empty(wishlist.ProductId);
    }
}
