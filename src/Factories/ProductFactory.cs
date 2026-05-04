using StoreApp.Models;
public class ProductFactory
{
    public Product CreateProduct(int id, string name, string desc, double price,
                                                string imageUrl, string category)
    {
        return new Product(id, name, desc, price, imageUrl, category);
    }
}