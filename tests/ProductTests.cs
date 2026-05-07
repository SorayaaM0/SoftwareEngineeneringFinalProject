using Xunit;
using StoreApp.Models;
public class ProductTests
{
    [Fact]
    public void Product_ShouldBeCreatedCorrectly()
    {
        var product = new Product();

        Assert.Equal(1, product.ProductId);
        Assert.Equal("Phone", product.Name);
        Assert.Equal(500, product.Price);
        Assert.False(product.IsWishlisted);
    }

    [Fact]
    public void UpdateDetails_ShouldRun()
    {
        var product = new Product();

        product.updateDetails();

        Assert.True(true);
    }
}
