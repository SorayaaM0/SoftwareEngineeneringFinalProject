using SQLite;
using StoreApp.Models;
using StoreApp.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StoreApp.src.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _db;

    

    public async Task InitAsync()
    {
        if (_db != null) return;
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "storeapp.db");
        _db = new SQLiteAsyncConnection(dbPath);
        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<ReportedProduct>();
        await _db.CreateTableAsync<Order>();
        await _db.CreateTableAsync<OrderItem>();
        await _db.CreateTableAsync<SecurityLog>();
        await _db.CreateTableAsync<Payment>();
        await _db.CreateTableAsync<Product>();
        await _db.CreateTableAsync<Wishlist>();
        await _db.CreateTableAsync<CartItem>();

        await SeedDataAsync();

    }

    private async Task SeedDataAsync()
    {
        // Only seed if tables are empty
        var productCount = await _db.Table<Product>().CountAsync();
        if (productCount == 0)
        {
            var products = new List<Product>
            {
                new Product { Name = "Classic White Shirt", Price = 19.99,
                                Category = "TOPS", ImageUrl = "shirt.jpg",
                                Description = "Soft cotton shirt." },
                new Product { Name = "Slim Fit Jeans", Price = 79.99,
                                Category = "BOTTOMS", ImageUrl = "jeans.jpg",
                                Description = "Modern fit jeans." },
                new Product { Name = "Pullover Hoodie", Price = 59.99,
                                Category = "OUTERWEAR", ImageUrl = "hoodie.jpg",
                            Description = "Soft fleece hoodie." }
            };

            foreach (var p in products)
                await _db.InsertAsync(p);
        }

        // Add test products so everyone has the same starting data

        var adminExists = await _db.Table<User>()
                            .Where(u => u.UserType == "Admin")
                            .FirstOrDefaultAsync();
        if (adminExists == null)
        {
            await _db.InsertAsync(new User
            {
                Name = "Admin User",
                Email = "admin@stylehub.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                UserType = "Admin"
            });
        }
    }


    public async Task<List<T>> GetAllAsync<T>() where T : new()
    {
        await InitAsync();
        return await _db.Table<T>().ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await _db.InsertAsync(user);
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        await InitAsync();

        return await _db.Table<User>()
                            .Where(u => u.Email == email)
                            .FirstOrDefaultAsync();
    }
    public async Task AddProduct(Product product)
    {
        await _db.InsertAsync(product);
    }
    public async Task<List<Product>> GetAllProducts()
    {
        return await _db.Table<Product>().ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        await InitAsync();
        return await _db.FindAsync<Product>(id);
    }

    public async Task AddWishlist(Wishlist wishlist)
    {
        await _db.InsertAsync(wishlist);
    }
    public async Task AddCartItem(CartItem cartItem)
    {
        await _db.InsertAsync(cartItem);
    }
    // Get all cart items for a user
    public async Task<List<CartItem>> GetCartAsync(int buyerId)
    {
        await InitAsync();
        return await _db.Table<CartItem>()
                        .Where(c => c.BuyerId == buyerId)
                        .ToListAsync();
    }

    // Check if item already exists in cart
    public async Task<CartItem> GetCartItemAsync(int buyerId, int productId)
    {
        await InitAsync();
        return await _db.Table<CartItem>()
                        .Where(c => c.BuyerId == buyerId && c.ProductId == productId)
                        .FirstOrDefaultAsync();
    }

    public async Task<Wishlist> GetWishlistEntryAsync(int buyerId, int productId)
    {
        await InitAsync();
        return await _db.Table<Wishlist>()
                        .Where(w => w.BuyerId == buyerId && w.ProductId == productId)
                        .FirstOrDefaultAsync();
    }

    public async Task<List<Wishlist>> GetWishlistAsync(int buyerId)
    {
        await InitAsync();
        return await _db.Table<Wishlist>()
                        .Where(w => w.BuyerId == buyerId)
                        .ToListAsync();
    }

    public async Task DeleteAsync(object item)
    {
        await _db.DeleteAsync(item);
    }
    public async Task UpdateAsync(object item)
    {
        await _db.UpdateAsync(item);
    }

    //Sellers only see their own products
    public async Task<List<Product>> GetProductsBySellerAsync(int sellerId)
    {
        await InitAsync();
        return await _db.Table<Product>()
                        .Where(p => p.SellerId == sellerId)
                        .ToListAsync();
    }

    public async Task<T> GetByIdAsync<T>(int id) where T : new()
    {
        await InitAsync();
        return await _db.FindAsync<T>(id);
    }

    public async Task<List<Order>> GetOrdersByBuyerAsync(int buyerId)
    {
        await InitAsync();
        return await _db.Table<Order>()
                        .Where(o => o.BuyerId == buyerId)
                        .OrderByDescending(o => o.OrderDate)
                        .ToListAsync();
    }
    public async Task<int> AddOrder(Order order)
    {
        await InitAsync();
        await _db.InsertAsync(order);

        // Fetch back the inserted order to get DB-assigned OrderId
        var inserted = await _db.Table<Order>()
                                .Where(o => o.BuyerId == order.BuyerId)
                                .OrderByDescending(o => o.OrderDate)
                                .FirstOrDefaultAsync();

        // Copy the generated ID back to the passed object
        order.OrderId = inserted.OrderId;
        return order.OrderId;
    }
     public async Task AddOrderItem(OrderItem orderItem)
    {
        await _db.InsertAsync(orderItem);
    }
     public async Task AddPayment(Payment payment)
    {
        await _db.InsertAsync(payment);
    }


    //Admin CRUDS

    // Get all unresolved reports with product and seller info
    public async Task<List<ReportedProduct>> GetUnresolvedReportsAsync()
    {
        await InitAsync();
        return await _db.Table<ReportedProduct>()
                        .Where(r => !r.IsResolved)
                        .ToListAsync();
    }

    public async Task<List<ReportedProduct>> GetReportsForProductAsync(int productId)
    {
        await InitAsync();
        return await _db.Table<ReportedProduct>()
                        .Where(r => r.ProductId == productId)
                        .ToListAsync();
    }

    public async Task ReportProductAsync(int productId, int reportedByUserId, string reason)
    {
        await InitAsync();

        // Prevent duplicate reports from same user
        var existing = await _db.Table<ReportedProduct>()
                                .Where(r => r.ProductId == productId
                                         && r.ReportedByUserId == reportedByUserId)
                                .FirstOrDefaultAsync();
        if (existing != null) return;

        await _db.InsertAsync(new ReportedProduct
        {
            ProductId = productId,
            ReportedByUserId = reportedByUserId,
            Reason = reason,
            ReportedAt = DateTime.Now,
            IsResolved = false
        });
    }

    //Security logs

    public async Task LogSecurityEventAsync(string type, string email,
                                        string details, int? userId = null)
    {
        await InitAsync();
        await _db.InsertAsync(new SecurityLog
        {
            Type = type,
            Email = email,
            UserId = userId,
            Details = details,
            OccurredAt = DateTime.Now,
            IsRead = false
        });
    }

    public async Task<List<SecurityLog>> GetSecurityLogsAsync()
    {
        await InitAsync();
        return await _db.Table<SecurityLog>()
                        .OrderByDescending(l => l.OccurredAt)
                        .ToListAsync();
    }

    public async Task ClearSecurityLogsAsync()
    {
        await InitAsync();
        await _db.DeleteAllAsync<SecurityLog>();
    }

    public async Task MarkLogReadAsync(SecurityLog log)
    {
        await InitAsync();
        log.IsRead = true;
        await _db.UpdateAsync(log);
    }

    //seller moderation

    public async Task<List<User>> GetAllSellersAsync()
    {
        await InitAsync();
        return await _db.Table<User>()
                        .Where(u => u.UserType == "Seller" && !u.IsBanned)
                        .ToListAsync();
    }

    public async Task<List<User>> GetBannedSellersAsync()
    {
        await InitAsync();
        return await _db.Table<User>()
                        .Where(u => u.UserType == "Seller" && u.IsBanned)
                        .ToListAsync();
    }

}



