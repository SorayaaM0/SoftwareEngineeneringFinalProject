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

        if (UserSession.IsLoggedIn &&
            !string.IsNullOrEmpty(UserSession.CurrentUser.ShippingAddress))
        {
            ShippingEntry.Text = UserSession.CurrentUser.ShippingAddress;
        }
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
            await DisplayAlert("Empty Cart",
                "Please add items before placing an order.", "OK");
            return;
        }

        // Validate shipping fields
        if (string.IsNullOrWhiteSpace(ShippingEntry.Text) ||
            string.IsNullOrWhiteSpace(CityEntry.Text) ||
            string.IsNullOrWhiteSpace(ZipCodeEntry.Text) ||
            string.IsNullOrWhiteSpace(StateEntry.Text) ||
            string.IsNullOrWhiteSpace(CountryEntry.Text))
        {
            await DisplayAlert("Missing Shipping Info",
                "Please fill in all shipping fields.", "OK");
            return;
        }

        // Validate payment fields
        if (string.IsNullOrWhiteSpace(CardholderNameEntry.Text) ||
            string.IsNullOrWhiteSpace(CardNumberEntry.Text) ||
            string.IsNullOrWhiteSpace(ExpirationEntry.Text) ||
            string.IsNullOrWhiteSpace(CSVEntry.Text))
        {
            await DisplayAlert("Missing Payment Info",
                "Please fill in all payment fields.", "OK");
            return;
        }

        // Basic card number validation
        if (CardNumberEntry.Text.Replace(" ", "").Length != 16)
        {
            await DisplayAlert("Invalid Card",
                "Please enter a valid 16-digit card number.", "OK");
            return;
        }

        double total = _displayItems.Sum(i => i.Price * i.Quantity);

        // Create order
        var order = new Order
        {
            BuyerId = UserSession.CurrentUser.Id,
            OrderDate = DateTime.Now,
            TotalAmount = total,
            Status = "Placed"
        };
        await _db.AddOrder(order);

        // Create order items
        foreach (var item in _displayItems)
        {
            await _db.AddOrderItem(new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = item.Product.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
                AddedAt = DateTime.Now
            });
        }

        // Create payment record
        await _db.AddPayment(new Payment
        {
            OrderId = order.OrderId,
            Amount = total,
            PaymentMethod = "Card",
            PaymentStatus = "Paid",
            PaidAt = DateTime.Now
        });

        // Save shipping address to user profile
        UserSession.CurrentUser.ShippingAddress =
            $"{ShippingEntry.Text}, {CityEntry.Text}, " +
            $"{StateEntry.Text} {ZipCodeEntry.Text}, {CountryEntry.Text}";
        await _db.UpdateAsync(UserSession.CurrentUser);

        // Clear cart
        var cartItems = await _db.GetCartAsync(UserSession.CurrentUser.Id);
        foreach (var cartItem in cartItems)
            await _db.DeleteAsync(cartItem);

        _displayItems.Clear();

        await DisplayAlert("Order Placed!",
            $"Order #{order.OrderId} confirmed!\nTotal: ${total:F2}", "OK");
        await Navigation.PopToRootAsync();
    }

    private async void OnContinueShoppingClicked(object sender, EventArgs e) => await Navigation.PopToRootAsync();
}