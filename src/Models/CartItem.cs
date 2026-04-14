using System.Collections.Generic;
using System.Linq;

public class CartItem
{
    public Product product { get; set; }
    public int quantity { get; set; }

    public CartItem(Product product, int quantity)
    {
        this.product = product;
        this.quantity = quantity;
    }

    public double getSubtotal()
    {
        return product.price * quantity;
    }
}