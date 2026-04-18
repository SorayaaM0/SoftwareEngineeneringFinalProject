namespace StoreApp.Models;

public class ShoppingCart
{
    public int cartId { get; set; }
    public List<CartItem> items { get; set; } = new List<CartItem>();

    public ShoppingCart(int cartId)
    {
        this.cartId = cartId;
    }

    public void addItem(Product product, int quantity)
    {
        items.Add(new CartItem(product, quantity));
    }

    public void removeItem(Product product)
    {
        items.RemoveAll( i => i.product.productId == product.productId);
    }

    public void updateQuantity(Product product, int quantity)
    {
        //looking through the cart to find the matching item
        var item = items.FirstOrDefault(i => i.product.productId == product.productId);
        if(item != null) //if it exists..
        {
            item.quantity = quantity; //update the quantity
        }
    }

    public double getTotal()
    {
        return items.Sum( i => i.getSubtotal());
    }

    public void clear()
    {
        items.Clear();
    }
}