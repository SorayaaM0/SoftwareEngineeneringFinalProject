//Creating an empty shopping cart for the user

public static class ShoppingCartFactory
{
    public static ShoppingCartFactory CreateCart(int cartId)
    {
        return new ShoppingCartFactory(cartId);
    }
}