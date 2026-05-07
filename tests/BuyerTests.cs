using Xunit;
using StoreApp.Models;


public class BuyerTests
{
    [Fact]
    public void Buyer_ShouldBeCreated()
    {
        var buyer = new Buyer();

        Assert.NotNull(buyer);
    }
}
