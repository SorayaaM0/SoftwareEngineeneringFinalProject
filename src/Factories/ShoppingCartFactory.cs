//Creating an empty shopping cart for the user

public class ShoppingCartFactory
{
    public ShoppingCart CreateShoppingCart(int id)
    {
        return new ShoppingCart(id);
    }
}