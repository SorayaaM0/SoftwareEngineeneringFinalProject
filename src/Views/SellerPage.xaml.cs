using StoreApp.Models;
using StoreApp.Services;
namespace StoreApp.Views;
using System.Collections.ObjectModel;

public partial class SellerPage : ContentPage
{
    // Local list to track products created during this session for testing
    public ObservableCollection<Product> MyProducts { get; set; } = new ObservableCollection<Product>();
    private Product selectedProduct;

    public SellerPage()
    {
        InitializeComponent();
        ProductsListView.ItemsSource = MyProducts;
        
        if (UserSession.CurrentUser is Seller seller)
        {
            StoreLabel.Text = seller.storeName;
        }
    }

    // When you click an item in the list, it fills the entry boxes
    private void OnProductSelected(object sender, SelectionChangedEventArgs e)
    {
        selectedProduct = e.CurrentSelection.FirstOrDefault() as Product;

        if (selectedProduct != null)
        {
            ProductNameEntry.Text = selectedProduct.name;
            ProductDescEntry.Text = selectedProduct.description;
            ProductPriceEntry.Text = selectedProduct.price.ToString();
            ProductImageEntry.Text = selectedProduct.imageUrl;
            ProductCategoryEntry.Text = selectedProduct.category;
        }
    }

    private async void OnCreateProductClicked(object sender, EventArgs e)
    {
        if (UserSession.CurrentUser is Seller seller)
        {
            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text) || !double.TryParse(ProductPriceEntry.Text, out double price))
            {
                await DisplayAlert("Error", "Please enter valid details", "OK");
                return;
            }

            var newProd = new Product(
                new Random().Next(1000, 9999),
                ProductNameEntry.Text,
                ProductDescEntry.Text ?? "No desc",
                price,
                ProductImageEntry.Text ?? "dotnet_bot.png",
                ProductCategoryEntry.Text ?? "General"
            );

            seller.createProduct(newProd);
            MyProducts.Add(newProd); // Adds to the visual list so you can select it later
            
            await DisplayAlert("Success", $"{newProd.name} listed!", "OK");
        }
    }

    private async void OnUpdateProductClicked(object sender, EventArgs e)
    {
        if (selectedProduct == null)
        {
            await DisplayAlert("Rainy Day", "Please select a product from the list first!", "OK");
            return;
        }

        if (UserSession.CurrentUser is Seller seller)
        {
            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text) || !double.TryParse(ProductPriceEntry.Text, out double newPrice) || newPrice <= 0)
            {
                await DisplayAlert("Validation Error", "Invalid price or name", "OK");
                return;
            }

            // Update the selected product's details
            selectedProduct.name = ProductNameEntry.Text;
            selectedProduct.description = ProductDescEntry.Text;
            selectedProduct.price = newPrice;
            selectedProduct.imageUrl = ProductImageEntry.Text;
            selectedProduct.category = ProductCategoryEntry.Text;

            seller.updateProduct(selectedProduct);
            selectedProduct.updateDetails();

            // Refresh the list view
            ProductsListView.ItemsSource = null;
            ProductsListView.ItemsSource = MyProducts;

            await DisplayAlert("Success", "Product updated!", "OK");
        }
    }

    private async void OnBackToStoreClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}