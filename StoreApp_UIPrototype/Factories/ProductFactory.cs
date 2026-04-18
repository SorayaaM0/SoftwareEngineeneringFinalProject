using StoreApp.Models;

public static class ProductFactory
{
    public static Product CreateProduct(int id, string name, string desc, double price,
                                                string imageUrl, string category)
    {
        return new Product(id, name, desc, price, imageUrl, category);
    }
}