using StoreApp.Models;
using StoreApp.src.Services;
public class ProductFactory
{
    private readonly DatabaseService _db;
    public ProductFactory(DatabaseService db)
    {
        _db = db;
    }
    public async Task<List<Product>> GetAllProductsAsync() => await _db.GetAllAsync<Product>();

    public async Task AddProductAsync(Product p) => await _db.AddProduct(p);

    public Product CreateProduct(string name, string desc, double price,
                                                string imageUrl, string category)
    {
        return new Product
        {
            Name = name,
            Description = desc,
            Price = price,
            ImageUrl = imageUrl,
            Category = category,
            IsWishlisted = false
        };
    }
}