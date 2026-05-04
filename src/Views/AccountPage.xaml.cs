using StoreApp.Models;
using StoreApp.Services;
namespace StoreApp.Views;

public partial class AccountPage : ContentPage
{
	private ShoppingCart _cart;

    public AccountPage(ShoppingCart cart)
	{
        InitializeComponent();
		_cart = cart;
		BindingContext = UserSession.CurrentUser;
		
    }

	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		UserSession.Logout();
		_cart.clear();
		await DisplayAlert("Logged Out", "You have been logged out.", "OK");
        await Navigation.PushAsync(new ProductPage());
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var user = UserSession.CurrentUser;
		if (user != null)
		{
			UserNameLabel.Text = user.name;
			UserEmailLabel.Text = user.email;
        }
    }
}