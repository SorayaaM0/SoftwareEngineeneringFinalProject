using StoreApp.Models;

namespace StoreApp.Services;

public static class UserSession
{
    public static User? CurrentUser { get; internal set; }
    public static bool IsLoggedIn => CurrentUser != null;
    public static bool Login(User user, string email, string password)
    {
        if (user.email == email && user.passwordHash == password)
        {
            CurrentUser = user;
            return true;
        }
        return false;
    }
    public static void Logout()
    {
        CurrentUser?.logout();
        CurrentUser = null;
    }
}