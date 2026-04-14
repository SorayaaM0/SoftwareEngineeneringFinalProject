public static class ProductFactory
{
    public static ProductFactory CreateProduct(int id, string name, string desc, double price,
                                                string imageUrl, string category)
    {
        return new ProductFactory(id, name, desc, price, imageUrl, category);
    }
}