using StoreApp.Models;

namespace StoreApp.src.Services;

public static class UserSession
{
    public static User? CurrentUser { get; private set; }
    public static bool IsLoggedIn => CurrentUser != null;
    public static bool Login(User user, string email, string password)
    {
        if (user == null)
        {
            return false;
        }

        bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if(!validPassword) 
        {
            return false;
        }

        CurrentUser = user;
        return true;
    }
    public static void Logout()
    {
        CurrentUser = null;
    }
}