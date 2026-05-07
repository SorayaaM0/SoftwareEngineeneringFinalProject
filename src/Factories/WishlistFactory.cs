//Creates a wishlist directly tied to a buyer
using StoreApp.src.Services;
public class WishlistFactory

{
    private readonly DatabaseService _db;

    public WishlistFactory(DatabaseService db)
    {
        _db = db;
    }

    public Wishlist CreateWishlist(int id, Buyer buyer)
    {
        return new Wishlist();
    }
}