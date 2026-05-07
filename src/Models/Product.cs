using SQLite;
namespace StoreApp.Models;

[Table("Products")]
public class Product
{
    [PrimaryKey, AutoIncrement]
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
    public string Category { get; set; }

    public bool IsWishlisted { get; set; }

    public int SellerId { get; set; } //FK to Seller

    public Product() { }


    public void updateDetails()
    {
        Console.WriteLine("Product details have been updated!");
    }
}