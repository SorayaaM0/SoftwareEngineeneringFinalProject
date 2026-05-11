// SecurityLogTests.cs — new file
using StoreApp.Models;
using StoreApp.src.Models;
using Xunit;

public class SecurityLogTests
{
    [Fact]
    public void SecurityLog_ShouldBeCreated()
    {
        var log = new SecurityLog();
        Assert.NotNull(log);
    }

    [Fact]
    public void SecurityLog_ShouldSetProperties()
    {
        var log = new SecurityLog
        {
            Type = "Failed Login",
            Email = "test@test.com",
            Details = "Wrong password",
            OccurredAt = DateTime.Now,
            IsRead = false
        };

        Assert.Equal("Failed Login", log.Type);
        Assert.False(log.IsRead);
    }

    // Rainy day
    [Fact]
    public void SecurityLog_DefaultIsRead_ShouldBeFalse()
    {
        var log = new SecurityLog();
        Assert.False(log.IsRead);
    }

    [Fact]
    public void SecurityLog_NullUserId_ShouldBeAllowed()
    {
        var log = new SecurityLog { UserId = null };
        Assert.Null(log.UserId);
    }
}