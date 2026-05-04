//Creates a wishlist directly tied to a buyer

public class WishlistFactory
{
    public Wishlist CreateWishlist(int id, Buyer buyer)
    {
        return new Wishlist(id, buyer);
    }
}