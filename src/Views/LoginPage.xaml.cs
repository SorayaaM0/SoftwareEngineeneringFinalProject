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

		if (EmailEntry.Text == "admin@test.com")
		{
			UserSession.CurrentUser = new Admin(
				0,
				"Admin User",
				"admin@testmail.com",
				"hashedpassword");
		}
    }

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RegisterPage());
    }

}