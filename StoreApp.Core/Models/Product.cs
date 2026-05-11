using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

    public string Size { get; set; }

    //public bool IsWishlisted { get; set; }

    public int SellerId { get; set; } //FK to Seller

    public Product() { }

    private bool _isWishlisted;
    public bool IsWishlisted
    {
        get=>_isWishlisted;
        set { if (_isWishlisted != value)
            {
                _isWishlisted = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public void updateDetails()
    {
        Console.WriteLine("Product details have been updated!");
    }
}