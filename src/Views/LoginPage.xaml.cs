using StoreApp.src.Services;
using StoreApp.Models;
namespace StoreApp.Views;

public partial class LoginPage : ContentPage
{
	private readonly DatabaseService _db;
    private readonly UserFactory _userFactory;
    public LoginPage(DatabaseService db)
	{
		InitializeComponent();
        _db = db;
        _userFactory = new UserFactory(db);
    }

	private async void OnLoginClicked(object sender, EventArgs e)
	{
		var email = EmailEntry.Text?.Trim() ?? "";
		var password = PasswordEntry.Text?.Trim() ?? "";
        try
        {


            var user = await _userFactory.LoginAsync(email, password);
            // UserFactory.LoginAsync — add after password check
            if (user.IsBanned)
            {
                await _db.LogSecurityEventAsync(
                    "Banned User Login Attempt",
                    email,
                    "Banned seller attempted to log in.",
                    user.Id);
                throw new Exception("This account has been suspended.");
            }
            UserSession.Login(user, user.Email, password);

            await DisplayAlert("Welcome", $"Hello, {user.Name}!", "OK");
            if (user.UserType == "Admin")
            {
                await Navigation.PushAsync(new AdminPage(_db));
            }
            else if (user.UserType == "Seller")
            {
                await Navigation.PushAsync(new SellerPage(_db));
            }
            else
            {
                await Navigation.PopToRootAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Login Failed", ex.Message, "OK");
        }
    }
	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RegisterPage(_db));
    }

}