using Xunit;

public class CartItemTests
{
    [Fact]
    public void CartItem_ShouldCalculateSubtotal()
    {
        var product = new Product(1, "Phone", "Smart phone", 500, "", "Electronics");
        var item = new CartItem(product, 2);

        Assert.Equal(1000, item.getSubtotal());
    }
}
