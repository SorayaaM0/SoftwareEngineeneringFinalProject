using StoreApp.Models;
using SQLite;
[Table("Sellers")]
public class Seller : User
{
  
    public string StoreName { get; set; }

    public Seller() { }

}