//Creates different types of users based on their roles
using StoreApp.Models;
public class UserFactory
{
    
    public User CreateUser(string role, int id, string name, string email, string passwordHash)
    {
        return role.ToLower() switch
        {
            "buyer" => new Buyer(id, name, email, passwordHash, "", ""),
            "seller" => new Seller(id, name, email, passwordHash, ""),
            "admin" => new Admin(id, name, email, passwordHash), _ =>
            throw new Exception("Invalid user role")
        };
    }
}