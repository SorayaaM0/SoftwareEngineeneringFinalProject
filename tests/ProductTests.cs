using Xunit;
using StoreApp.Models;
public class ProductTests
{
    [Fact]
    public void Product_ShouldBeCreatedCorrectly()
    {
        var product = new Product(1, "Phone", "Smart phone", 500, "img.jpg", "Electronics");

        Assert.Equal(1, product.productId);
        Assert.Equal("Phone", product.name);
        Assert.Equal(500, product.price);
        Assert.False(product.isWishlisted);
    }

    [Fact]
    public void UpdateDetails_ShouldRun()
    {
        var product = new Product(1, "Phone", "Smart phone", 500, "img.jpg", "Electronics");

        product.updateDetails();

        Assert.True(true);
    }
}
