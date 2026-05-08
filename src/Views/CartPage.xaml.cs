using StoreApp.Models;
using StoreApp.src.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Views;

public partial class CartPage : ContentPage
{
    private readonly DatabaseService _db;
    private List<CartItemDisplay> _displayItems = new();
    
    // 1. Define the _cart field that was missing
    private ShoppingCart _cart = new ShoppingCart();

    public CartPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        if (!UserSession.IsLoggedIn)
        {
            bool login = await DisplayAlert("Not Logged In", "Please log in to view your cart.", "Login", "Cancel");
            if (login)
            {
                await Navigation.PushAsync(new LoginPage(_db));
            } 
            else
            {
                await Navigation.PopAsync();
            }
            return;
        }
        await LoadCartFromDbAsync();
    }

    private async Task LoadCartFromDbAsync()
    {
        if (!UserSession.IsLoggedIn) return;

        var items = await _db.GetCartAsync(UserSession.CurrentUser.Id);
        _displayItems.Clear();
        
        // 2. Clear the local _cart object so it stays in sync with the DB
        _cart.clear();

        foreach (var item in items)
        {
            var product = await _db.GetByIdAsync<Product>(item.ProductId);
            if (product != null)
            {
                var display = new CartItemDisplay
                {
                    CartItem = item,
                    Quantity = item.Quantity,
                    ImageUrl = product.ImageUrl,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price
                };
                _displayItems.Add(display);
                
                // 3. Add the item to our _cart object for the CheckoutView
                _cart.addItem(product, item.Quantity);
            }
        }
        CartCollection.ItemsSource = null;
        CartCollection.ItemsSource = _displayItems;
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        double total = _displayItems.Sum(i => i.Price * i.Quantity);
        TotalLabel.Text = $"${total:F2}";
    }

    private async void OnIncreaseQuantity(object sender, EventArgs e)
    {
        var display = (CartItemDisplay)((Button)sender).CommandParameter;
        if (display == null) return;

        display.CartItem.Quantity += 1;
        await _db.UpdateAsync(display.CartItem);
        await LoadCartFromDbAsync();
    }

    private async void OnDecreaseQuantity(object sender, EventArgs e)
    {
        var display = (CartItemDisplay)((Button)sender).CommandParameter;
        if (display == null || display.CartItem.Quantity <= 1) return;

        display.CartItem.Quantity -= 1;
        await _db.UpdateAsync(display.CartItem);
        await LoadCartFromDbAsync();
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var display = (CartItemDisplay)((Button)sender).CommandParameter;
        if (display == null) return;

        await _db.DeleteAsync(display.CartItem);
        await LoadCartFromDbAsync();
    }

    private async void OnCheckoutClicked(object sender, EventArgs e)
    {
        if (!_displayItems.Any())
        {
            await DisplayAlert("Empty Cart", "Add items before checking out.", "OK");
            return;
        }

        // 4. Now '_cart' is properly defined and populated, so this line will work
        await Navigation.PushAsync(new CheckoutView(_db, _cart));
    }

    private async void OnCartItemTapped(object sender, EventArgs e)
    {
        if (sender is Grid grid && 
            grid.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tap &&
            tap.CommandParameter is CartItemDisplay displayItem) // Changed to CartItemDisplay to match the UI binding
        {
            var product = await _db.GetByIdAsync<Product>(displayItem.CartItem.ProductId);
            if (product != null)
            {
                await Navigation.PushAsync(new ProductDetailPage(product, _db));
            }
        }
    }
}
