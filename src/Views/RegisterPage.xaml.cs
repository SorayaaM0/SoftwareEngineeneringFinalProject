using Microsoft.Maui.ApplicationModel.Communication;
using StoreApp.Models;
using StoreApp.src.Services;
using System.Xml.Linq;

namespace StoreApp.Views;

public partial class RegisterPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly UserFactory _userFactory;
    public RegisterPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        _userFactory = new UserFactory(_db);
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var password = PasswordEntry.Text?.Trim() ?? "";
        if(password.Length < 6 || !password.Any(char.IsDigit) || !password.Any(char.IsUpper))
        {
            await DisplayAlert("Error", "Password must be at least 6 characters long, contain a number, and an uppercase letter.", "OK");
            return;
        }
        if (password != ConfirmPasswordEntry.Text?.Trim())
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }
        var name = NameEntry.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "Name must not be empty", "OK");
            return;
        }
        var email = EmailEntry.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(email))
        {
            await DisplayAlert("Error", "Email must not be empty", "OK");
            return;
        }
        try
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            
            if (IsSellerCheckBox.IsChecked)
            {
                var storeName = StoreNameEntry.Text?.Trim() ?? "";
                if (string.IsNullOrEmpty(storeName))
                {
                    await DisplayAlert("Error", "Store name must not be empty for sellers.", "OK");
                    return;
                }

                var seller = new User
                {
                    Name = name,
                    Email = email,
                    PasswordHash = passwordHash,
                    UserType = "Seller",
                    StoreName = storeName
                };
                await _db.AddUserAsync(seller);
            } else
            {
                var buyer = new User
                {
                    Name = name,
                    Email = email,
                    PasswordHash = passwordHash,
                    UserType = "Buyer"
                };
                await _db.AddUserAsync(buyer);
            }
            var savedUser = await _db.GetUserByEmailAsync(email);
            UserSession.Login(savedUser, email, password);

            await DisplayAlert("Success", "Account created!", "OK");

            if (savedUser.UserType == "Seller")
                await Navigation.PushAsync(new SellerPage(_db));
            else
                await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}