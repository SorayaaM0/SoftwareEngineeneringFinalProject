using StoreApp.Models;
using StoreApp.src.Services;
namespace StoreApp.Views;

public partial class CheckoutView : ContentPage
{
	private readonly DatabaseService _db;
    private ShoppingCart _cart;

    public CheckoutView(DatabaseService db, ShoppingCart cart)
	{
		InitializeComponent();
		_db = db;
		_cart = cart;
		
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

		//Create the Order record
		var newOrder = new Order
		{
			BuyerId = UserSession.CurrentUser.Id, // Link to the logged-in buyer 
			OrderDate = DateTime.Now,
			TotalAmount = (double)_cart.getTotal(),
			Status = "Placed"
		};

		//Save to the SQLite database
		await _db.AddOrderAsync(newOrder);

		//Clear the database cart items for this user
		var cartItems = await _db.GetCartAsync(UserSession.CurrentUser.Id);
		foreach (var item in cartItems)
		{
			await _db.DeleteAsync(item);
		}

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