using System.Linq;
using Xunit;
using StoreApp.Models;
public class ShoppingCartTests
{
    [Fact]
    public void AddItem_ShouldAddProduct()
    {
        var cart = new ShoppingCart(1);
        var product = new Product(1, "Phone", "Smart phone", 500, "", "Electronics");

        cart.addItem(product, 2);

        Assert.Single(cart.items);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveProduct()
    {
        var cart = new ShoppingCart(1);
        var product = new Product(1, "Phone", "Smart phone", 500, "", "Electronics");

        cart.addItem(product, 2);
        cart.removeItem(product);

        Assert.Empty(cart.items);
    }

    [Fact]
    public void GetTotal_ShouldCalculateCorrectly()
    {
        var cart = new ShoppingCart(1);
        var product = new Product(1, "Phone", "Smart phone", 500, "", "Electronics");

        cart.addItem(product, 2);

        Assert.Equal(1000, cart.getTotal());
    }
}
