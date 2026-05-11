using StoreApp.Models;
using StoreApp.src.Services;
using StoreApp.src.Views;
namespace StoreApp.Views;

public partial class AccountPage : ContentPage
{
	private readonly DatabaseService _db;
    private bool _sidebarOpen = false;


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
		
		UpdateUIForUser();
        UpdateSidebar();
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

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        if (UserSession.IsLoggedIn)
        {
            //await
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
			await Navigation.PushAsync(new OrderHistoryPage(_db));
		}
		else
		{
			await DisplayAlert("Access Denied", "Only customers can view order history.", "OK");
		}
    }

    private async void OnMenuClicked(object sender, EventArgs e) => await OpenSidebarAsync();

    private async void OnOverlayTapped(object sender, EventArgs e) => await CloseSidebarAsync();

    private async void OnCloseSidebarClicked(object sender, EventArgs e) => await CloseSidebarAsync();

    private async Task OpenSidebarAsync()
    {
        if (UserSession.IsLoggedIn)
        {
            SidebarUserLabel.Text = $"Hello, {UserSession.CurrentUser.Name}!";
            SidebarEmailLabel.Text = UserSession.CurrentUser.Email;
            SidebarLoginBtn.IsVisible = false;
            SidebarLogoutBtn.IsVisible = true;

        }
        else
        {
            SidebarUserLabel.Text = "Welcome, Guest!";
            SidebarEmailLabel.Text = "Please log in to access more features.";
            SidebarLoginBtn.IsVisible = true;
            SidebarLogoutBtn.IsVisible = false;
        }
        SidebarOverlay.IsVisible = true;
        _sidebarOpen = true;

        await Task.WhenAll(
            SidebarOverlay.FadeTo(0.5, 250),
            SidebarPanel.TranslateTo(0, 0, 250, Easing.CubicOut)
        );
    }

    private async Task CloseSidebarAsync()
    {
        _sidebarOpen = false;

        await Task.WhenAll(
            SidebarOverlay.FadeTo(0, 200),
            SidebarPanel.TranslateTo(300, 0, 200, Easing.CubicIn)
        );

        SidebarOverlay.IsVisible = false;
    }

    private void UpdateSidebar()
    {
        if (UserSession.IsLoggedIn)
        {
            SidebarUserLabel.Text = $"Hello, {UserSession.CurrentUser.Name}!";
            SidebarEmailLabel.Text = UserSession.CurrentUser.Email;
            SidebarLoginBtn.IsVisible = false;
            SidebarLogoutBtn.IsVisible = true;
        }
        else
        {
            SidebarUserLabel.Text = "Welcome, Guest!";
            SidebarEmailLabel.Text = "Please log in to access more features.";
            SidebarLoginBtn.IsVisible = true;
            SidebarLogoutBtn.IsVisible = false;
        }
    }
    private async void OnSidebarLoginClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PushAsync(new LoginPage(_db));
    }

    private async void OnSidebarLogoutClicked(object sender, EventArgs e)
    {

        await CloseSidebarAsync();
        UserSession.Logout();
        await DisplayAlert("Logged Out", "You have been logged out successfully.", "OK");
        await Navigation.PopToRootAsync();
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PopToRootAsync();
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PushAsync(new CartPage(_db));
    }

    private async void OnCollectionsClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PushAsync(new WishlistPage(_db));
    }

    private async void OnAccountClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        if (UserSession.IsLoggedIn)
            await Navigation.PushAsync(new AccountPage(_db));
        else
            await Navigation.PushAsync(new LoginPage(_db));
    }
}