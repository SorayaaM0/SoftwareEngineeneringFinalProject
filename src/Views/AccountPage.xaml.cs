using StoreApp.Models;
using StoreApp.src.Services;
namespace StoreApp.Views;

public partial class AccountPage : ContentPage
{
	private readonly DatabaseService _db;
    

    public AccountPage(DatabaseService db)
	{
		InitializeComponent();
        _db = db;
		BindingContext = UserSession.CurrentUser;
		
    }

	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		UserSession.Logout();
		
		await DisplayAlert("Logged Out", "You have been logged out.", "OK");
        await Navigation.PushAsync(new ProductPage(_db));
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var user = UserSession.CurrentUser;
		if (user != null)
		{
			UserNameLabel.Text = user.Name;
			UserEmailLabel.Text = user.Email;
        }
		UpdateUIForUser();
    }

	private void UpdateUIForUser()
	{
        switch (UserSession.CurrentUser?.UserType)
        {
            case "Admin":
                DashboardButton.Text = "Admin Dashboard";
                DashboardButton.IsVisible = true;
                break;
            case "Seller":
                DashboardButton.Text = "Seller Dashboard";
                DashboardButton.IsVisible = true;
                break;
            default:
                DashboardButton.IsVisible = false;
                break;
        }
    }

	private async void OnDashboardClicked(object sender, EventArgs e)
	{
		switch (UserSession.CurrentUser?.UserType)
		{
			case "Admin":
				await Navigation.PushAsync(new AdminPage(_db));
				break;
			case "Seller":
				await Navigation.PushAsync(new SellerPage(_db));
				break;
			default:
				break;
		}
	}

	private async void OnMyOrdersClicked(object sender, EventArgs e)
	{
		if (UserSession.CurrentUser.UserType == "Buyer" || UserSession.CurrentUser.UserType == "Seller")
		{
			//await Navigation.PushAsync(new OrderHistoryPage(_db));
		}
		else
		{
			await DisplayAlert("Access Denied", "Only customers can view order history.", "OK");
		}
    }
}