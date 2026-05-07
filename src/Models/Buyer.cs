using StoreApp.Models;
using SQLite;


[Table("Buyers")]
public class Buyer : User
{
    [Column("ShippingAddress")]
    public string shippingAddress { get; set; }
    [Column("BillingAddress")]
    public string billingAddress { get; set; }

    public Buyer() { }


    public void viewOrderHistory()
    {
        Console.WriteLine("Viewing order history...");
    }

    public void addToWishlist(Product product)
    {
        Console.WriteLine($"{product.Name} added to wishlist");
    }

}
