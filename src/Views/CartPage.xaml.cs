using MyStoreApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyStoreApp.Views;

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
        await DisplayAlert("Checkout", $"Your total is ${_cart.getTotal():F2}. Proceeding to Payment...", "OK");
    }

    private void RefreshUI()
    {
        //tells the UI to refresh the list and recalculate the total
        CartCollection.ItemsSource = null;
        CartCollection.ItemsSource = _cart.items;
        UpdateTotal();
    }
}