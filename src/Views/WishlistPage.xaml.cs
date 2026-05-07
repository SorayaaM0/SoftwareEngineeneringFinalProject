using StoreApp.Models;
using StoreApp.src.Services;
namespace StoreApp.Views;

public partial class WishlistPage : ContentPage
{
    private readonly DatabaseService _db;
    

    public WishlistPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!UserSession.IsLoggedIn)
        {
            bool login = await DisplayAlert("Not Logged In", "Please log in to view your wishlist.", "Login", "Cancel");
            if (login)
            {
                await Navigation.PushAsync(new LoginPage(_db));
            } else
            {
                await Navigation.PopAsync();
            }
                return;
        }
        await LoadWishlistFromDbAsync();
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var product = (Product)((Button)sender).CommandParameter;
        if (product == null) return;

        // Find and delete the wishlist entry from DB
        var entry = await _db.GetWishlistEntryAsync(
            UserSession.CurrentUser.Id, product.ProductId);

        if (entry != null)
            await _db.DeleteAsync(entry);

        // Reload to reflect removal
        await LoadWishlistFromDbAsync();
    }

    public async Task LoadWishlistFromDbAsync()
    {
        if (!UserSession.IsLoggedIn) return;
        var wishlistEntries = await _db.GetWishlistAsync(UserSession.CurrentUser.Id);
        var products = new List<Product>();
        foreach (var entry in wishlistEntries)
        {
            var product = await _db.GetByIdAsync<Product>(entry.ProductId);
            if (product != null)
            {
                products.Add(product);
            }
        }
        WishlistCollection.ItemsSource = products;
    }
}