using System.Linq;
using Xunit;
using StoreApp.Models;
public class ShoppingCartTests
{
    [Fact]
    public void AddItem_ShouldAddProduct()
    {
        var cart = new ShoppingCart();
        var product = new Product();

        cart.addItem(product, 2);

        Assert.Single(cart.Items);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveProduct()
    {
        var cart = new ShoppingCart();
        var product = new Product();

        cart.addItem(product, 2);
        cart.removeItem(product.ProductId);

        Assert.Empty(cart.Items);
    }

    [Fact]
    public void GetTotal_ShouldCalculateCorrectly()
    {
        var cart = new ShoppingCart();
        var product = new Product();

        cart.addItem(product, 2);

        Assert.Equal(1000, cart.getTotal());
    }
}
