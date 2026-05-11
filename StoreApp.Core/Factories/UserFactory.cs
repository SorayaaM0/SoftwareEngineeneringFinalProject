//Creates different types of users based on their roles
using StoreApp.Models;
using StoreApp.src.Services;
public class UserFactory
{
    private readonly DatabaseService _db;
    public UserFactory(DatabaseService db)
    {
        _db = db;
    }

    public async Task<User> CreateUserAsync(string role, string name, string email, string passwordHash, string storename = "")
    {
        var existingUser = await _db.GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            throw new Exception("Email already in use.");
        }

        User newUser = role.ToLower() switch
        {
            "buyer" => new User 
            { 
                Name = name, 
                Email = email, 
                PasswordHash = passwordHash, 
                UserType = "Buyer" 
            },
            "admin" => new User 
            {  
                Name = name, 
                Email = email, 
                PasswordHash = passwordHash, 
                UserType = "Admin" 
            },

            "seller" => new Seller 
            { 
                Name = name, 
                Email = email, 
                PasswordHash = passwordHash,
                StoreName = storename,
                UserType = "Seller" 
            },
            _ => throw new Exception("Invalid user role.")
        };

        await _db.AddUserAsync(newUser);
        return await _db.GetUserByEmailAsync(email);
    }

    public async Task<User> LoginAsync(string email, string passwordHash)
    {
        var user = await _db.GetUserByEmailAsync(email);
        if (user == null)
        {
            await _db.LogSecurityEventAsync(
                "Failed Login Attempt",
                email,
                "No account found with this email.");
            throw new Exception("No Account Found");
        }
        bool valid = BCrypt.Net.BCrypt.Verify(passwordHash, user.PasswordHash);
        if (!valid)
        {
            await _db.LogSecurityEventAsync(
                "Failed Login Attempt",
                email,
                "Incorrect Password entered.");
            throw new Exception("Invalid password.");
        }
        return user;
    }

}