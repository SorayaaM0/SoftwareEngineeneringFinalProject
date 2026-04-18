namespace StoreApp.Models;

public class User
{
    public int userId { get; set;}
    public string name { get; set; }
    public string email { get; set; }
    public string passwordHash {get; set; }

    public User(int userId, string name, string email, string passwordHash)
    {
        this.userId = userId;
        this.name = name;
        this.email = email;
        this.passwordHash = passwordHash;
    }

    public void register()
    {
        //placeholder 
        Console.WriteLine("Registration Successful!");
    }
    
    public bool login(string email, string password)
    {
        //placeholder
        return this.email == email;
    }

    public void logout()
    {
        Console.WriteLine("Logged out");
    }
}