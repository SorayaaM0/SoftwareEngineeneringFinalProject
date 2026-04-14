//This will be the completed purchase

public class Order
{
    public int orderId { get; set; }
    public Buyer buyer { get; set; }
    public List<CartItem> items { get; set; }
    public double totalAmount { get; set; }
    public string status { get; set; }

    public Order(int orderId, Buyer buyer, List<CartItem> items)
    {
        this.orderId = orderId;
        this.buyer = buyer;
        this.items = items;
        this.totalAmount = items.Sum(i => i.getSubtotal());
        this.status = "Pending..";
    }

    public void placeOrder()
    {
        status = "Placed";
    }

    public void updateStatus(string newStatus)
    {
        status = newStatus;
    }
}