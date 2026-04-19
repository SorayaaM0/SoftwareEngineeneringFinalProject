/* Functional Requirements:

FR5. Add/Remove products to wishlist
FR13. WishlistFactory association */

using System.Collections.Generic;
namespace StoreApp.Models;

public class Wishlist
{
    public int wishlistId { get; set; }
    public Buyer buyer { get; set; }
    public List<Product> products { get; set; } = new List<Product>();

    public Wishlist(int wishlistId, Buyer buyer)
    {
        this.wishlistId = wishlistId;
        this.buyer = buyer;
    }

    public void addProduct(Product product)
    {
        products.Add(product);
    }

    public void removeProduct(Product product)
    {
        products.RemoveAll(p => p.productId == product.productId);
    }
}