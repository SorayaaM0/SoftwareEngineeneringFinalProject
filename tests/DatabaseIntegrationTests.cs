using Xunit;
using StoreApp.Models;
using StoreApp.src.Services;

public class DatabaseIntegrationTests : IAsyncLifetime
{
    private DatabaseService _db;
    private string _dbPath;

    public async Task InitializeAsync()
    {
        // Unique path per test
        _dbPath = Path.Combine(
            Path.GetTempPath(),
            $"storetest_{Guid.NewGuid():N}.db");

        _db = new DatabaseService(_dbPath);
        await _db.InitAsync();
    }

    public async Task DisposeAsync()
    {
        await _db.DisposeAsync();

        // Retry delete — SQLite sometimes holds file briefly
        for (int i = 0; i < 5; i++)
        {
            try
            {
                if (File.Exists(_dbPath)) File.Delete(_dbPath);
                break;
            }
            catch
            {
                await Task.Delay(100);
            }
        }
    }

    // ── User CRUD ───────────────────────────────────────────

    [Fact]
    public async Task InsertUser_ShouldPersistToDatabase()
    {
        var user = new User
        {
            Name = "Test Buyer",
            Email = "buyer@test.com",
            PasswordHash = "hash123",
            UserType = "Buyer"
        };
        await _db.AddUserAsync(user);
        var result = await _db.GetUserByEmailAsync("buyer@test.com");

        Assert.NotNull(result);
        Assert.Equal("Test Buyer", result.Name);
        Assert.Equal("Buyer", result.UserType);
    }

    [Fact]
    public async Task GetUserByEmail_NonExistent_ShouldReturnNull()
    {
        var result = await _db.GetUserByEmailAsync("nobody@test.com");
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUser_ShouldPersistChanges()
    {
        var user = new User
        {
            Name = "Old Name",
            Email = "update@test.com",
            PasswordHash = "hash",
            UserType = "Buyer"
        };
        await _db.AddUserAsync(user);

        var saved = await _db.GetUserByEmailAsync("update@test.com");
        saved.Name = "New Name";
        await _db.UpdateAsync(saved);

        var updated = await _db.GetUserByEmailAsync("update@test.com");
        Assert.Equal("New Name", updated.Name);
    }

    [Fact]
    public async Task BanUser_ShouldPersistBanStatus()
    {
        var user = new User
        {
            Name = "Seller",
            Email = "seller@test.com",
            PasswordHash = "hash",
            UserType = "Seller",
            IsBanned = false
        };
        await _db.AddUserAsync(user);

        var saved = await _db.GetUserByEmailAsync("seller@test.com");
        saved.IsBanned = true;
        await _db.UpdateAsync(saved);

        var banned = await _db.GetUserByEmailAsync("seller@test.com");
        Assert.True(banned.IsBanned);
    }

    // ── Product CRUD ────────────────────────────────────────

    [Fact]
    public async Task InsertProduct_ShouldPersistToDatabase()
    {
        var product = new Product
        {
            Name = "Test Shirt",
            Price = 29.99,
            Category = "TOPS",
            ImageUrl = "shirt.jpg",
            SellerId = 1
        };
        await _db.AddProduct(product);

        // Only query for this specific product
        var all = await _db.GetProductsBySellerAsync(1);
        Assert.Single(all);
        Assert.Equal("Test Shirt", all[0].Name);
    }

    [Fact]
    public async Task GetProductsBySeller_ShouldReturnOnlySellerProducts()
    {
        await _db.AddProduct(new Product { Name = "P1", Price = 10, SellerId = 1 });
        await _db.AddProduct(new Product { Name = "P2", Price = 20, SellerId = 1 });
        await _db.AddProduct(new Product { Name = "P3", Price = 30, SellerId = 2 });

        var seller1Products = await _db.GetProductsBySellerAsync(1);
        Assert.Equal(2, seller1Products.Count);
        Assert.All(seller1Products, p => Assert.Equal(1, p.SellerId));
    }

    [Fact]
    public async Task DeleteProduct_ShouldRemoveFromDatabase()
    {
        var product = new Product { Name = "Delete Me", Price = 10, SellerId = 99 };
        await _db.AddProduct(product);

        // Get only seller 99's products
        var saved = (await _db.GetProductsBySellerAsync(99))[0];
        await _db.DeleteAsync(saved);

        var remaining = await _db.GetProductsBySellerAsync(99);
        Assert.Empty(remaining);
    }

    // ── Cart CRUD ───────────────────────────────────────────

    [Fact]
    public async Task AddCartItem_ShouldPersistToDatabase()
    {
        // Use unique buyer ID per test to avoid cross-test pollution
        int uniqueBuyerId = new Random().Next(10000, 99999);

        var item = new CartItem
        {
            BuyerId = uniqueBuyerId,
            ProductId = 1,
            Quantity = 2,
            Price = 19.99
        };
        await _db.AddCartItem(item);

        var cart = await _db.GetCartAsync(uniqueBuyerId);
        Assert.Single(cart);
        Assert.Equal(2, cart[0].Quantity);
    }

    [Fact]
    public async Task GetCart_ShouldOnlyReturnBuyerItems()
    {
        int buyer1 = new Random().Next(10000, 49999);
        int buyer2 = new Random().Next(50000, 99999);

        await _db.AddCartItem(new CartItem
        { BuyerId = buyer1, ProductId = 1, Quantity = 1, Price = 10 });
        await _db.AddCartItem(new CartItem
        { BuyerId = buyer1, ProductId = 2, Quantity = 2, Price = 20 });
        await _db.AddCartItem(new CartItem
        { BuyerId = buyer2, ProductId = 3, Quantity = 1, Price = 30 });

        var buyer1Cart = await _db.GetCartAsync(buyer1);
        Assert.Equal(2, buyer1Cart.Count);
        Assert.All(buyer1Cart, item => Assert.Equal(buyer1, item.BuyerId));
    }

    [Fact]
    public async Task UpdateCartItem_ShouldChangeQuantity()
    {
        int uniqueBuyerId = new Random().Next(10000, 99999);
        var item = new CartItem
        { BuyerId = uniqueBuyerId, ProductId = 1, Quantity = 1, Price = 10 };
        await _db.AddCartItem(item);

        var saved = (await _db.GetCartAsync(uniqueBuyerId))[0];
        saved.Quantity = 5;
        await _db.UpdateAsync(saved);

        var updated = (await _db.GetCartAsync(uniqueBuyerId))[0];
        Assert.Equal(5, updated.Quantity);
    }

    [Fact]
    public async Task DeleteCartItem_ShouldRemoveFromCart()
    {
        int uniqueBuyerId = new Random().Next(10000, 99999);
        var item = new CartItem
        { BuyerId = uniqueBuyerId, ProductId = 1, Quantity = 1, Price = 10 };
        await _db.AddCartItem(item);

        var saved = (await _db.GetCartAsync(uniqueBuyerId))[0];
        await _db.DeleteAsync(saved);

        var cart = await _db.GetCartAsync(uniqueBuyerId);
        Assert.Empty(cart);
    }

    // ── Order Flow ──────────────────────────────────────────

    [Fact]
    public async Task PlaceOrder_ShouldCreateOrderAndItems()
    {
        int uniqueBuyerId = new Random().Next(10000, 99999);
        var order = new Order
        {
            BuyerId = uniqueBuyerId,
            OrderDate = DateTime.Now,
            TotalAmount = 49.98,
            Status = "Placed"
        };
        await _db.AddOrder(order);

        await _db.AddOrderItem(new OrderItem
        {
            OrderId = order.OrderId,
            ProductId = 1,
            Quantity = 2,
            Price = 19.99,
            AddedAt = DateTime.Now
        });

        var orders = await _db.GetOrdersByBuyerAsync(uniqueBuyerId);
        Assert.Single(orders);
        Assert.Equal(49.98, orders[0].TotalAmount);
    }

    [Fact]
    public async Task GetOrdersByBuyer_ShouldOnlyReturnBuyerOrders()
    {
        int buyer1 = new Random().Next(10000, 49999);
        int buyer2 = new Random().Next(50000, 99999);

        await _db.AddOrder(new Order
        {
            BuyerId = buyer1,
            TotalAmount = 10,
            Status = "Placed",
            OrderDate = DateTime.Now
        });
        await _db.AddOrder(new Order
        {
            BuyerId = buyer1,
            TotalAmount = 20,
            Status = "Placed",
            OrderDate = DateTime.Now
        });
        await _db.AddOrder(new Order
        {
            BuyerId = buyer2,
            TotalAmount = 30,
            Status = "Placed",
            OrderDate = DateTime.Now
        });

        var buyer1Orders = await _db.GetOrdersByBuyerAsync(buyer1);
        Assert.Equal(2, buyer1Orders.Count);
        Assert.All(buyer1Orders, o => Assert.Equal(buyer1, o.BuyerId));
    }

    // ── Wishlist ────────────────────────────────────────────

    [Fact]
    public async Task AddWishlistEntry_ShouldPersist()
    {
        int uniqueBuyerId = new Random().Next(10000, 99999);
        var entry = new Wishlist { BuyerId = uniqueBuyerId, ProductId = 5 };
        await _db.AddWishlist(entry);

        var wishlist = await _db.GetWishlistAsync(uniqueBuyerId);
        Assert.Single(wishlist);
        Assert.Equal(5, wishlist[0].ProductId);
    }

    [Fact]
    public async Task GetWishlistEntry_ShouldReturnCorrectEntry()
    {
        int uniqueBuyerId = new Random().Next(10000, 99999);
        await _db.AddWishlist(new Wishlist { BuyerId = uniqueBuyerId, ProductId = 3 });

        var entry = await _db.GetWishlistEntryAsync(uniqueBuyerId, 3);
        Assert.NotNull(entry);
        Assert.Equal(uniqueBuyerId, entry.BuyerId);
        Assert.Equal(3, entry.ProductId);
    }

    [Fact]
    public async Task GetWishlistEntry_NonExistent_ShouldReturnNull()
    {
        var entry = await _db.GetWishlistEntryAsync(99999, 99999);
        Assert.Null(entry);
    }

    // ── Security Logs ───────────────────────────────────────

    [Fact]
    public async Task LogSecurityEvent_ShouldPersist()
    {
        await _db.LogSecurityEventAsync(
            "Failed Login", "bad@test.com", "Wrong password", null);

        var logs = await _db.GetSecurityLogsAsync();
        Assert.Single(logs);
        Assert.Equal("Failed Login", logs[0].Type);
        Assert.False(logs[0].IsRead);
    }

    [Fact]
    public async Task ClearSecurityLogs_ShouldRemoveAll()
    {
        await _db.LogSecurityEventAsync("Event 1", "a@test.com", "detail");
        await _db.LogSecurityEventAsync("Event 2", "b@test.com", "detail");

        await _db.ClearSecurityLogsAsync();

        var logs = await _db.GetSecurityLogsAsync();
        Assert.Empty(logs);
    }

    // ── Reported Products ───────────────────────────────────

    [Fact]
    public async Task ReportProduct_ShouldPersist()
    {
        await _db.ReportProductAsync(1, 2, "Inappropriate content");

        var reports = await _db.GetUnresolvedReportsAsync();
        Assert.Single(reports);
        Assert.Equal("Inappropriate content", reports[0].Reason);
    }

    [Fact]
    public async Task ReportProduct_DuplicateReport_ShouldNotAddSecond()
    {
        await _db.ReportProductAsync(1, 2, "Reason 1");
        await _db.ReportProductAsync(1, 2, "Reason 2");

        var reports = await _db.GetUnresolvedReportsAsync();
        Assert.Single(reports);
    }
}