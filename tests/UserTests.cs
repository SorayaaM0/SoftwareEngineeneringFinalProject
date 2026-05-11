// UserTests.cs — new file
using Xunit;
using StoreApp.Models;

public class UserTests
{
    // Sunny day
    [Fact]
    public void User_ShouldBeCreated()
    {
        var user = new User();
        Assert.NotNull(user);
    }

    [Fact]
    public void User_ShouldSetProperties()
    {
        var user = new User
        {
            Name = "Test User",
            Email = "test@test.com",
            UserType = "Buyer",
            PasswordHash = "hashedpassword"
        };

        Assert.Equal("Test User", user.Name);
        Assert.Equal("Buyer", user.UserType);
        Assert.False(user.IsBanned);
    }

    [Fact]
    public void User_IsBanned_DefaultShouldBeFalse()
    {
        var user = new User();
        Assert.False(user.IsBanned);
    }

    [Fact]
    public void User_CanBeBanned()
    {
        var user = new User { IsBanned = true };
        Assert.True(user.IsBanned);
    }

    // Rainy day
    [Fact]
    public void User_NullEmail_ShouldBeNull()
    {
        var user = new User();
        Assert.Null(user.Email);
    }

    [Fact]
    public void User_AdminType_ShouldNotBeBuyer()
    {
        var user = new User { UserType = "Admin" };
        Assert.NotEqual("Buyer", user.UserType);
    }
}