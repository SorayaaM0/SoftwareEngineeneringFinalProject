namespace StoreApp.Models;

public class Buyer : User
{
    public string shippingAddress { get; set; }
    public string billingAddress { get; set; }

    public Buyer(int userId, string name, string email, string passwordHash,
                    string shippingAddress, string billingAddress)
                    : base(userId, name, email, passwordHash)
    {
        this.shippingAddress = shippingAddress;
        this.billingAddress = billingAddress;
    }

    public void viewOrderHistory()
    {
        Console.WriteLine("Viewing order history...");
    }

    public void addToWishlist(Product product)
    {
        Console.WriteLine($"{product.name} added to wishlist");
    }
}