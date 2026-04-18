//Creates a wishlist directly tied to a buyer

using StoreApp.Models;

public static class WishlistFactory
{
    public static Wishlist CreateWishlist(int id, Buyer buyer)
    {
        return new Wishlist(id, buyer);
    }
}