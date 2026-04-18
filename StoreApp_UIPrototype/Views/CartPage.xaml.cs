using StoreApp.Models;
using StoreApp.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace StoreApp.Views;

public partial class CartPage : ContentPage
{
    private ShoppingCart _cart;

    public CartPage(ShoppingCart cart)
    {
        InitializeComponent();
        
        _cart = cart;

        //this binds the CollectionView to the internal list of items in the cart
        CartCollection.ItemsSource = _cart.items;
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        double total = _cart.getTotal();
        TotalLabel.Text = $"${total:F2}";
    }

    private void OnIncreaseQuantity(object sender, EventArgs e)
    {
        var cartItem = (CartItem)((Button)sender).CommandParameter;
        if (cartItem != null)
        {
            // uses updateQuantity() method
            _cart.updateQuantity(cartItem.product, cartItem.quantity + 1);
            RefreshUI();
        }
    }

    private void OnDecreaseQuantity(object sender, EventArgs e)
    {
        var cartItem = (CartItem)((Button)sender).CommandParameter;
        if (cartItem != null && cartItem.quantity > 1)
        {
            _cart.updateQuantity(cartItem.product, cartItem.quantity - 1);
            RefreshUI();
        }
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        var cartItem = (CartItem)((Button)sender).CommandParameter;
        if (cartItem != null)
        {
            //uses removeItem() method
            _cart.removeItem(cartItem.product);
            RefreshUI();
        }
    }

    private async void OnCheckoutClicked(object sender, EventArgs e)
    {
        if (UserSession.IsLoggedIn)
        {
            await DisplayAlert("Checkout", $"Your total is ${_cart.getTotal():F2}. Proceeding to Payment...", "OK");
            await Navigation.PushAsync(new CheckoutView(_cart));
        }
        else
        {
            bool login = await DisplayAlert("Login Required", "You need to be logged in to proceed to checkout. Do you want to log in now?", "Yes", "No");
            if (login)
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }
        
    }

    private async void OnCartItemTapped(object sender, EventArgs e)
    {
        if (sender is Grid grid && 
            grid.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tap &&
            tap.CommandParameter is CartItem cartItem)
        {
            await Navigation.PushAsync(new ProductDetailPage(cartItem.product, _cart));
        }
    }

    private void RefreshUI()
    {
        //tells the UI to refresh the list and recalculate the total
        CartCollection.ItemsSource = null;
        CartCollection.ItemsSource = _cart.items;
        UpdateTotal();
    }
}