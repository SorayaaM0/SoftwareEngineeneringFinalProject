using StoreApp.Models;
using StoreApp.src.Services;
namespace StoreApp.Views;

public partial class CheckoutView : ContentPage
{
	private readonly DatabaseService _db;
	private List<CartItemDisplay> _displayItems = new();

    public CheckoutView(DatabaseService db)
	{
		InitializeComponent();
		_db = db;
		
	
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadCheckoutData();
	}
	
	private async Task LoadCheckoutData()
	{
		if(!UserSession.IsLoggedIn) return;
		var cartItems = await _db.GetCartAsync(UserSession.CurrentUser.Id);
		_displayItems.Clear();
		foreach (var item in cartItems)
		{
			var product = await _db.GetProductByIdAsync(item.ProductId);
			if (product != null)
			{
				_displayItems.Add(new CartItemDisplay
				{
					Name = product.Name,
					Description = product.Description,
					ImageUrl = product.ImageUrl,
					Price = product.Price,
					Quantity = item.Quantity,
					Product = product,
					CartItem = item

                });
            }
        }

		OrderItems.ItemsSource = null;
		OrderItems.ItemsSource = _displayItems;

		UpdateTotal();
    }

	private void UpdateTotal()
	{
		double total = _displayItems.Sum(i => i.Price * i.Quantity);
		TotalLabel.Text = $"Total: ${total:F2}";
    }

	private async void OnPlaceOrderClicked(object sender, EventArgs e)
	{
        if (!_displayItems.Any())
		{
			await DisplayAlert("Empty Cart", "Your cart is empty. Please add items before placing an order.", "OK");
			return;
        }

        // Create a new order
        var order = new Order
		{
			BuyerId = UserSession.CurrentUser.Id,
			OrderDate = DateTime.Now,
			TotalAmount = _displayItems.Sum(i => i.Price * i.Quantity),
			Status = "Placed"
		};
		await _db.AddOrder(order);

		// Create OrderItems
		foreach (var item in _displayItems)
		{
			var orderItem = new OrderItem
			{
				OrderId = order.OrderId,
				ProductId = item.Product.ProductId,
				Quantity = item.Quantity,
				Price = item.Price,
				AddedAt = DateTime.Now
            };
			await _db.AddOrderItem(orderItem);
        }

        var cartItems = await _db.GetCartAsync(UserSession.CurrentUser.Id);
		foreach (var cartItem in cartItems)
		{
			await _db.DeleteAsync(cartItem);
        }
        _displayItems.Clear();

        await DisplayAlert("Order Placed", "Your order has been placed successfully!", "OK");

        await Navigation.PopToRootAsync();
    }
}