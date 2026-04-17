using MyStoreApp.Models;

namespace MyStoreApp.Views;

public partial class WishlistPage : ContentPage
{
    private Wishlist _wishlist;

    public WishlistPage(Wishlist wishlist)
    {
        InitializeComponent();
        _wishlist = wishlist;
        WishlistCollection.ItemsSource = _wishlist.products;
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        var product = (Product)((Button)sender).CommandParameter;
        
        if (product != null)
        {
            //update the product state
            product.isWishlisted = false;

            //uses removeProduct method
            _wishlist.removeProduct(product);

            //refreshes the UI list
            WishlistCollection.ItemsSource = null;
            WishlistCollection.ItemsSource = _wishlist.products;
        }
    }
}