using System.Collections.Generic;
using System.Linq;
using StoreApp.Models;
using SQLite;

[Table("CartItems")]
public class CartItem
{
    [PrimaryKey, AutoIncrement]
    public int CartItemId { get; set; }
    public int BuyerId { get; set; } //Foreign key to Buyer
    public int ProductId { get; set; } //Foreign key to Product
    public int Quantity { get; set; }
    public double Price { get; set; }

    [Ignore]
    public string ProductName { get; set; }
    [Ignore]
    public string ProductDescription { get; set; }
    [Ignore]
    public string ProductImageUrl { get; set; }

    [Ignore]
    public double ProductPrice { get; set; }

    public CartItem() { }



    public double getSubtotal()
    {
        // Placeholder for actual product price retrieval
        double productPrice = Price; // Replace with actual product price retrieval logic
        return productPrice * Quantity;
    }
}