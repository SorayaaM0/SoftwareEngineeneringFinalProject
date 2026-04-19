/* Functional Requirements:
 
FR6. Create and update products (role based) */

namespace StoreApp.Models;

public class Seller : User
{
    public string storeName { get; set; }

    public Seller(int userId, string name, string email, string passwordHash, string storeName)
                    : base(userId, name, email, passwordHash)
    {
        this.storeName = storeName;
    }

    public void createProduct(Product p)
    {
        Console.WriteLine($"Product {p.name} created");
    }

    public void updateProduct(Product p)
    {
        Console.WriteLine($"Product {p.name} updated");
    }
}