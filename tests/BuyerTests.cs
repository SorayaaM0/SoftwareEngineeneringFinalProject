using Xunit;
using StoreApp.Models;


public class BuyerTests
{
    [Fact]
    public void Buyer_ShouldBeCreated()
    {
        var buyer = new Buyer(1, "Test User", "test@test.com", "password", "123 Ship St", "123 Bill St");

        Assert.NotNull(buyer);
    }
}
