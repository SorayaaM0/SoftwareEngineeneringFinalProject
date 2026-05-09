using StoreApp.Models;
using StoreApp.src.Services;
using System.Threading.Tasks;

namespace StoreApp.Views;

public partial class AdminPage : ContentPage
{
	private readonly DatabaseService _db;

    public AdminPage(DatabaseService db)
	{
		InitializeComponent();
        _db = db;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if(UserSession.CurrentUser?.UserType != "Admin")
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
        await Navigation.PushAsync(new AdminProductModerationPage(_db));
    }

    private async void OnManageUsersClicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new AdminSellerModerationPage());
    }

    private async void OnUnauthorizedAccessClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminSecurityLogsPage());
    }

	private async void OnBackClicked(object sender, EventArgs e)
	{
		await Navigation.PopToRootAsync();
	}

}