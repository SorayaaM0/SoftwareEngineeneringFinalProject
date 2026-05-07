using StoreApp.Models;
using StoreApp.src.Services;
namespace StoreApp.Views;

public partial class CheckoutView : ContentPage
{
	private readonly DatabaseService _db;
    private ShoppingCart _cart;

    public CheckoutView(DatabaseService db)
	{
		InitializeComponent();
		_db = db;
		
		OrderItems.ItemsSource = _cart.Items;
		TotalLabel.Text = $"Total: ${_cart.getTotal():F2}";

    }
	
	private async void OnPlaceOrderClicked(object sender, EventArgs e)
	{
		if (_cart.Items.Count == 0)
		{
			await DisplayAlert("Error", "Your cart is empty.", "OK");
			return;
		}
		// Simulate order processing
		await DisplayAlert("Success", "Your order has been placed!", "OK");
		_cart.clear();
        await Navigation.PopToRootAsync();
    }

	private async void OnCheckoutComplete()
	{
		var cartItems = await _db.GetCartAsync(UserSession.CurrentUser.Id);
		foreach (var item in cartItems)
		{
			await _db.DeleteAsync(item);
        }
		_cart.clear();
    }
}