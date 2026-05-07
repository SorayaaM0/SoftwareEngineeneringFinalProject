using StoreApp.Models;
using SQLite;

[Table("Payments")]
public class Payment
{
    [PrimaryKey, AutoIncrement]
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public double Amount { get; set;}
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime PaidAt { get; set; }

    public Payment() { }

}