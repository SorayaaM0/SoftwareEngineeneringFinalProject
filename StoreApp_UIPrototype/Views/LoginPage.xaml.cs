using StoreApp.Services;
using StoreApp.Models;
namespace StoreApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private async void OnLoginClicked(object sender,EventArgs e)
	{
		var email = EmailEntry.Text;
		var password = PasswordEntry.Text;

		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlert("Error", "Please enter both email and password.", "OK");
			return;
        }

		

		//Stub User
		var user = new User(
			userId: 1,
			name: "John Doe",
			email: "test@testmail.com",
			passwordHash: "hashed_password");

		if (UserSession.Login(user,email, password))
		{
			//go back to product page
			await Navigation.PopAsync();
        }

		if (EmailEntry.Text == "admin@testmail.com")
		{
			var admin = UserFactory.CreateUser(
				"admin",
				0,
				"Admin User",
				"admin@testmail.com",
				"adminpass");

			UserSession.Login(admin, email, password);
			await Navigation.PushAsync(new AdminPage());
			return;

		}

    }

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RegisterPage());
    }

}