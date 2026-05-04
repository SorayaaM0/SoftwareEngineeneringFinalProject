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

        if (string.IsNullOrWhiteSpace(name) ||
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

        User newUser;

        if (IsSellerCheckBox.IsChecked)
        {
            var storeName = StoreNameEntry.Text;
            if (string.IsNullOrWhiteSpace(storeName))
            {
                await DisplayAlert("Error", "Sellers must provide a Store Name.", "OK");
                return;
            }

            newUser = new Seller(
                userId: new Random().Next(1, 10000),
                name: name,
                email: email,
                passwordHash: "stubbed_hash",
                storeName: storeName
            );
        }
        else
        {
            newUser = new User(
                userId: new Random().Next(1, 10000),
                name: name,
                email: email,
                passwordHash: "stubbed_hash"
            );
        }

        newUser.register();

        // Only change: Pass the stubbed hash to the Login method to ensure the session starts.
        if (UserSession.Login(newUser, email, "stubbed_hash"))
        {
            await DisplayAlert("Success", "Registration successful!", "OK");

            if (newUser is Seller)
            {
                await Navigation.PushAsync(new SellerPage());
            }
            else
            {
                await Navigation.PopToRootAsync();
            }
        }
    }
}