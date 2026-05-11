// ProductTests.cs
using Xunit;
using StoreApp.Models;

public class ProductTests
{
    // Sunny day
    [Fact]
    public void Product_ShouldBeCreated()
    {
        var product = new Product();
        Assert.NotNull(product);
    }

    [Fact]
    public void Product_ShouldSetPropertiesCorrectly()
    {
        var product = new Product
        {
            Name = "Test Shirt",
            Price = 29.99,
            Category = "TOPS",
            ImageUrl = "shirt.jpg",
            Description = "A test shirt"
        };

        Assert.Equal("Test Shirt", product.Name);
        Assert.Equal(29.99, product.Price);
        Assert.Equal("TOPS", product.Category);
    }

    [Fact]
    public void Product_IsWishlisted_DefaultShouldBeFalse()
    {
        var product = new Product();
        Assert.False(product.IsWishlisted);
    }

    [Fact]
    public void Product_IsWishlisted_ShouldToggle()
    {
        var product = new Product();
        product.IsWishlisted = true;
        Assert.True(product.IsWishlisted);
        product.IsWishlisted = false;
        Assert.False(product.IsWishlisted);
    }

    [Fact]
    public void UpdateDetails_ShouldRunWithoutException()
    {
        var product = new Product();
        var ex = Record.Exception(() => product.updateDetails());
        Assert.Null(ex);
    }

    // Rainy day
    [Fact]
    public void Product_NegativePrice_ShouldStillStore()
    {
        // DB doesn't enforce positive — validation is UI responsibility
        var product = new Product { Price = -10 };
        Assert.Equal(-10, product.Price);
    }

    [Fact]
    public void Product_NullName_ShouldBeNull()
    {
        var product = new Product();
        Assert.Null(product.Name);
    }
}