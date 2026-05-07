using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreApp.Models;
using SQLite;
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
        
        await _db.CreateTableAsync<Order>();
        await _db.CreateTableAsync<OrderItem>();
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

}



