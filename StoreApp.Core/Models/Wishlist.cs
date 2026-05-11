using System.Collections.Generic;
using StoreApp.Models;
using SQLite;
[Table("Wishlists")]
public class Wishlist
{
    [PrimaryKey, AutoIncrement]
    public int WishlistId { get; set; }
    public int BuyerId { get; set; }
    public int ProductId { get; set; }

    public Wishlist() { }

    public void addProduct(int productId)
    {
        if (productId <= 0)
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }
        else
        {
            Console.WriteLine($"Adding product {productId} to wishlist.");
            // Logic to add the product to the wishlist
        }
    }
    public void removeProduct(int productId)
    {
        if (productId <= 0)
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }
        else
        {
            Console.WriteLine($"Removing product {productId} from wishlist.");
            // Logic to remove the product from the wishlist

        }
    }

   }