using StoreApp.Models;

namespace StoreApp.Views;

public partial class CheckoutView : ContentPage
{
	private ShoppingCart _cart;

    public CheckoutView(ShoppingCart cart)
	{
		InitializeComponent();
		_cart = cart;
		OrderItems.ItemsSource = _cart.items;
		TotalLabel.Text = $"Total: ${_cart.getTotal():F2}";

    }
	
	private async void OnPlaceOrderClicked(object sender, EventArgs e)
	{
		if (_cart.items.Count == 0)
		{
			await DisplayAlert("Error", "Your cart is empty.", "OK");
			return;
		}
		// Simulate order processing
		await DisplayAlert("Success", "Your order has been placed!", "OK");
		_cart.clear();
        await Navigation.PopToRootAsync();
    }
}