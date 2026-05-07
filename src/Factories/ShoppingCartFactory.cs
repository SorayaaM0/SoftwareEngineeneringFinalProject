//Creating an empty shopping cart for the user
using StoreApp.src.Services;
public class ShoppingCartFactory
{
    private readonly DatabaseService _db;
    public ShoppingCartFactory(DatabaseService db)
    {
        _db = db;
    }
    public ShoppingCart CreateShoppingCart()
    {
        return new ShoppingCart();
    }
}