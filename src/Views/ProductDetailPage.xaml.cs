using StoreApp.Models;
using StoreApp.src.Services;
using System.Threading.Tasks;
namespace StoreApp.Views;

public partial class ProductDetailPage : ContentPage
{
	private readonly DatabaseService _db;
    private Product _product;
	private ShoppingCart _cart;
    public ProductDetailPage(Product product, DatabaseService db)
	{
		InitializeComponent();
        _db = db;

		_product = product;

        ProductImage.Source = _product.ImageUrl;
		ProductName.Text = _product.Name;
		ProductPrice.Text = $"${_product.Price:F2}";
		ProductDescription.Text = _product.Description;
    }

	private async void OnAddToCartClicked(object sender, EventArgs e)
	{
        if (!UserSession.IsLoggedIn)
        {
            await DisplayAlert("Login Required", "Please log in first.", "OK");
            return;
        }

        var existing = await _db.GetCartItemAsync(
            UserSession.CurrentUser.Id, _product.ProductId);

        if (existing != null)
        {
            existing.Quantity += 1;
            await _db.UpdateAsync(existing);
        }
        else
        {
            await _db.AddCartItem(new CartItem
            {
                BuyerId = UserSession.CurrentUser.Id,  // tied to THIS user
                ProductId = _product.ProductId,
                Quantity = 1,
                Price = _product.Price
            });
        }

        await DisplayAlert("Added", $"{_product.Name} added to cart!", "OK");

    }
}