using StoreApp.Models;
using StoreApp.Services;
using System.Threading.Tasks;
namespace StoreApp.Views;

public partial class ProductDetailPage : ContentPage
{
	private Product _product;
	private ShoppingCart _cart;
    public ProductDetailPage(Product product, ShoppingCart cart)
	{
		InitializeComponent();

		_product = product;
		_cart = cart;

        ProductImage.Source = _product.imageUrl;
		ProductName.Text = _product.name;
		ProductPrice.Text = $"${_product.price:F2}";
		ProductDescription.Text = _product.description;
    }

	private async void OnAddToCartClicked(object sender, EventArgs e)
	{
        _cart.addItem(_product,1);
		await DisplayAlert("Success", "Product added to cart!", "OK");

    }
}