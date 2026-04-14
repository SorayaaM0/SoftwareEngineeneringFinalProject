//Creates a wishlist directly tied to a buyer

public static class WishlistFactory
{
    public static WishlistFactory CreateWishlist(int id, Buyer buyer)
    {
        return new WishlistFactory(id, buyer);
    }
}