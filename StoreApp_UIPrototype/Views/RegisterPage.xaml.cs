using StoreApp.Services;
using StoreApp.Models;

namespace StoreApp.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		var name = NameEntry.Text;
		var email = EmailEntry.Text;
		var password = PasswordEntry.Text;
		var confirmPassword = ConfirmPasswordEntry.Text;
		
		//Basic validation
		if(string.IsNullOrWhiteSpace(name) ||
			string.IsNullOrWhiteSpace(email) ||
			string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlert("Error", "Please fill in all fields.", "OK");
			return;
        }
		if (password != confirmPassword)
		{
			await DisplayAlert("Error", "Passwords do not match.", "OK");
			return;
        }
		var newUser = new User(
			userId: new Random().Next(1, 10000), //Backend
			name: name,
			email: email,
			passwordHash: "stubbed_hash"
			); 

		newUser.register();
		UserSession.Login(newUser, email, password);

		await DisplayAlert("Success", "Registration successful!", "OK");
		await Navigation.PopToRootAsync();
    }
}