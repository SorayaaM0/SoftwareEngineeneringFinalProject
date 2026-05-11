// ShoppingCartTests.cs
using Xunit;
using StoreApp.Models;

public class ShoppingCartTests
{
    // Sunny day
    [Fact]
    public void Cart_ShouldStartEmpty()
    {
        var cart = new ShoppingCart();
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void AddItem_ShouldAddProduct()
    {
        var cart = new ShoppingCart();
        var product = new Product { ProductId = 1, Price = 10.00 };
        cart.addItem(product, 2);
        Assert.Single(cart.Items);
    }

    [Fact]
    public void AddItem_ShouldIncrementQuantityIfAlreadyInCart()
    {
        var cart = new ShoppingCart();
        var product = new Product { ProductId = 1, Price = 10.00 };
        cart.addItem(product, 1);
        cart.addItem(product, 2);
        Assert.Single(cart.Items);
        Assert.Equal(3, cart.Items[0].Quantity);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveProduct()
    {
        var cart = new ShoppingCart();
        var product = new Product { ProductId = 1, Price = 10.00 };
        cart.addItem(product, 2);
        cart.removeItem(product.ProductId);
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void GetTotal_ShouldCalculateCorrectly()
    {
        var cart = new ShoppingCart();
        var p1 = new Product { ProductId = 1, Price = 10.00 };
        var p2 = new Product { ProductId = 2, Price = 20.00 };
        cart.addItem(p1, 2);  // 20
        cart.addItem(p2, 1);  // 20
        Assert.Equal(40.00, cart.getTotal());
    }

    [Fact]
    public void UpdateQuantity_ShouldChangeQuantity()
    {
        var cart = new ShoppingCart();
        var product = new Product { ProductId = 1, Price = 10.00 };
        cart.addItem(product, 1);
        cart.updateQuantity(product.ProductId, 5);
        Assert.Equal(5, cart.Items[0].Quantity);
    }

    [Fact]
    public void Clear_ShouldEmptyCart()
    {
        var cart = new ShoppingCart();
        var product = new Product { ProductId = 1, Price = 10.00 };
        cart.addItem(product, 2);
        cart.clear();
        Assert.Empty(cart.Items);
    }

    // Rainy day
    [Fact]
    public void RemoveItem_ProductNotInCart_ShouldNotThrow()
    {
        var cart = new ShoppingCart();
        var ex = Record.Exception(() => cart.removeItem(999));
        Assert.Null(ex);
    }

    [Fact]
    public void GetTotal_EmptyCart_ShouldBeZero()
    {
        var cart = new ShoppingCart();
        Assert.Equal(0, cart.getTotal());
    }

    [Fact]
    public void UpdateQuantity_NonExistentProduct_ShouldNotThrow()
    {
        var cart = new ShoppingCart();
        var ex = Record.Exception(() => cart.updateQuantity(999, 5));
        Assert.Null(ex);
    }
}