/* Functional Requirements:
 
FR12. Payment Processing
FR11. Updates order after payment */

namespace StoreApp.Models;

public class Payment
{
    public int paymentId { get; set; }
    public Order order { get; set; }
    public double amount { get; set;}
    public string paymentMethod { get; set; }
    public string paymentStatus { get; set; }

    public Payment(int paymentId, Order order, string paymentMethod)
    {
        this.paymentId = paymentId;
        this.order = order;
        this.amount = order.totalAmount;
        this.paymentMethod = paymentMethod;
        this.paymentStatus = "Pending";
    }

    public void processPayment()
    {
        paymentStatus = "Completed";
        order.updateStatus("Paid");
    }
}