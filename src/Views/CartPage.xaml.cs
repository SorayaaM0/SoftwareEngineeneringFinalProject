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
            } else
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

        // Load cart items from DB
        var items = await _db.GetCartAsync(UserSession.CurrentUser.Id);
        _displayItems.Clear();
        foreach (var item in items)
        {
            var product = await _db.GetByIdAsync<Product>(item.ProductId);
            if (product != null)
            {
                _displayItems.Add(new CartItemDisplay
                {
                    CartItem = item,
                    Quantity = item.Quantity,
                    ImageUrl = product.ImageUrl,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price
                });
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
        if (display == null)
        {
            return;
        }

        display.CartItem.Quantity += 1;
        await _db.UpdateAsync(display.CartItem);
        await LoadCartFromDbAsync();


    }

    private async void OnDecreaseQuantity(object sender, EventArgs e)
    {
        var display = (CartItemDisplay)((Button)sender).CommandParameter;
        if (display == null || display.CartItem.Quantity <= 1)
        {
            return;
        }

        display.CartItem.Quantity -= 1;
        await _db.UpdateAsync(display.CartItem);
        await LoadCartFromDbAsync();
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var display = (CartItemDisplay)((Button)sender).CommandParameter;
        if (display == null) 
        { 
            return; 
        }    

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
        await Navigation.PushAsync(new CheckoutView(_db));

    }

    private async void OnCartItemTapped(object sender, EventArgs e)
    {
        if (sender is Grid grid && 
            grid.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tap &&
            tap.CommandParameter is CartItem cartItem)
        {
            var product = await _db.GetByIdAsync<Product>(cartItem.ProductId);
            if (product != null)
            {
                await Navigation.PushAsync(new ProductDetailPage(product,_db));
            }
        }
    }


}