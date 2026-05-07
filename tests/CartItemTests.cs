using Xunit;
using StoreApp.Models;
public class CartItemTests
{
    [Fact]
    public void CartItem_ShouldCalculateSubtotal()
    {
        var product = new Product();
        var item = new CartItem();

        Assert.Equal(1000, item.getSubtotal());
    }
}
