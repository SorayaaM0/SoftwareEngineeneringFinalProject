using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StoreApp.Models;


[Table("OrderItems")]
public class OrderItem
{
    [PrimaryKey, AutoIncrement]
    public int OrderItemId { get; set; }
    public int OrderId { get; set; } //Foreign key to Order
    public int ProductId { get; set; } //Foreign key to Product
    public int Quantity { get; set; }
    public double Price { get; set; }
    public DateTime AddedAt { get; set; }
    public OrderItem() { }
    public double getSubtotal()
    {
        return Price * Quantity;
    }
}

