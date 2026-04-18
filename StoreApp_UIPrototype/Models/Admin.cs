namespace StoreApp.Models;


public class Admin : User
{
    public Admin(int userId, string name, string email, string passwordHash)
                : base(userId, name, email, passwordHash)
    {
        
    }

    public void moderateListing(Product product)
    {
        Console.WriteLine($"Moderating product {product.name}");
    }

    public void banSeller(Seller seller)
    {
        Console.WriteLine($"Seller {seller.name} has been banned.");
    }
}