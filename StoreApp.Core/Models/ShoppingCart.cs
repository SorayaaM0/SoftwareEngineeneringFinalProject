using StoreApp.Models;
using SQLite;

[Table("ShoppingCart")]
public class ShoppingCart
{
    [PrimaryKey]
    public int CartId { get; set; }

    [Ignore]
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    public ShoppingCart() { }

    public void addItem(Product product, int quantity)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == product.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity; //if the item is already in the cart, just update the quantity
            return;
        }
        else
        {
            Items.Add(new CartItem
            {
                ProductId = product.ProductId,
                Quantity = quantity,
                Price = product.Price

            });

        }
    }

    public void removeItem(int productId)
    {
        Items.RemoveAll(i => i.ProductId == productId);
    }

    public void updateQuantity(int productId, int quantity)
    {
        //looking through the cart to find the matching item
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if(item != null) //if it exists..
        {
            item.Quantity = quantity; //update the quantity
        }
    }

    public void clear()
    {
        Items.Clear();
    }

    public double getTotal()
    {
        return Items.Sum( i => i.Price * i.Quantity);
    }
}