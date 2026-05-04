using StoreApp.Models;
using StoreApp.Services;
using System.Threading.Tasks;

namespace StoreApp.Views;

public partial class AdminPage : ContentPage
{


	public AdminPage()
	{
		InitializeComponent();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if(UserSession.CurrentUser is not Admin)
		{
			await DisplayAlert(
				"Access Denied",
				"You are not authorized for this page",
				"OK"
			);
			await Navigation.PopAsync();

		}
	}

    private async void OnModerateListingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminProductModerationPage());
    }

    private async void OnManageUsersClicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new AdminSellerModerationPage());
    }

    private async void OnUnauthorizedAccessClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminSecurityLogsPage());
    }

}