using SQLite;
namespace StoreApp.Models;


[Table("Admins")]
public class Admin : User
{
    public Admin() { }

    public void moderateListing(Product product)
    {
        Console.WriteLine($"Moderating product {product.Name}");
    }

    public void banSeller(Seller seller)
    {
        Console.WriteLine($"Seller {seller.Name} has been banned.");
    }

}



