//Creating an empty shopping cart for the user

using StoreApp.Models;

public static class ShoppingCartFactory
{
    public static ShoppingCart CreateCart(int cartId)
    {
        return new ShoppingCart(cartId);
    }
}