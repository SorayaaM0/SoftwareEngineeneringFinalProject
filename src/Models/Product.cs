public class Product
{
    public int productId { get; set;}
    public string name { get; set; }
    public string description {get; set; }
    public double price { get; set; }
    public string imageUrl { get; set; }
    public string category {get; set; }

    public Product(int productId, string name, string description, double price,
                    string imageUrl, string category)
    {
        this.productId = productId;
        this.name = name;
        this.description = description;
        this.price = price;
        this.imageUrl = imageUrl;
        this.category = category;
    }

    public void updateDetails()
    {
        Console.WriteLine("Product details have been updated!");
    }
}