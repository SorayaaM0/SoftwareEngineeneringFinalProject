//This will be the completed purchase
using SQLite;
using StoreApp.Models;

[Table("Orders")]
public class Order
{
    [PrimaryKey, AutoIncrement]
    public int OrderId { get; set; }
    
    public int BuyerId { get; set; } //Foreign key to Buyer

    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public string Status { get; set; }

    public Order() { }


    public void updateStatus(string newStatus)
    {
        Status = newStatus;
    }
}