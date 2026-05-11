using SQLite;
namespace StoreApp.Models;

[Table("Users")]
public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string UserType { get; set; }

    //buyer specific
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string Phone { get; set; }

    //seller specific

    public bool IsBanned {  get; set; }
    public string StoreName { get; set; }
    public User() { }

    public void register()
    {
        //placeholder 
        Console.WriteLine("Registration Successful!");
    }
    
    public bool login(string email, string password)
    {
        //placeholder
        return true;
    }

    public void logout()
    {
        Console.WriteLine("Logged out");
    }

}
