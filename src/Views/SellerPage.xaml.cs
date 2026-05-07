using StoreApp.Models;
using StoreApp.src.Services;
namespace StoreApp.Views;
using System.Collections.ObjectModel;

public partial class SellerPage : ContentPage
{
 

    // Local list to track products created during this session for testing
    public ObservableCollection<Product> MyProducts { get; set; } = new();
    private Product selectedProduct;
    private readonly DatabaseService _db;
    private string _pickedImagePath = null;

    public SellerPage(DatabaseService db)
    {
        InitializeComponent();
        ProductsListView.ItemsSource = MyProducts;

        if (UserSession.CurrentUser.UserType == "Seller")
        {
            StoreLabel.Text = (UserSession.CurrentUser as User)?.StoreName ?? "My Store";
        }

        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if(UserSession.CurrentUser?.UserType != "Seller")
        {
            await DisplayAlert("Access Denied", "You must be logged in as a seller to access this page.", "OK");
            await Navigation.PopAsync();
            return;
        }
        StoreLabel.Text = (UserSession.CurrentUser as Seller)?.StoreName ?? "My Store";
        await LoadProductAsync();
    }

    private async Task LoadProductAsync()
    {
        
        var products = await _db.GetProductsBySellerAsync(UserSession.CurrentUser.Id);
        MyProducts.Clear();
        foreach (var prod in products)
        {
            MyProducts.Add(prod);
        }
        
    }

    // When you click an item in the list, it fills the entry boxes
    private void OnProductSelected(object sender, SelectionChangedEventArgs e)
    {
        selectedProduct = e.CurrentSelection.FirstOrDefault() as Product;

        if (selectedProduct != null)
        {
            ProductNameEntry.Text = selectedProduct.Name;
            ProductDescEntry.Text = selectedProduct.Description;
            ProductPriceEntry.Text = selectedProduct.Price.ToString();
            ProductImageEntry.Text = selectedProduct.ImageUrl;
            ProductCategoryEntry.Text = selectedProduct.Category;
        }
    }

    private async void OnCreateProductClicked(object sender, EventArgs e)
    {
        if (UserSession.CurrentUser is Seller seller)
        {
            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text) || !double.TryParse(ProductPriceEntry.Text, out double price) || price <= 0)
            {
                await DisplayAlert("Error", "Please enter valid details", "OK");
                return;
            }

            var newProd = new Product
            {
                Name = ProductNameEntry.Text.Trim(),
                Description = ProductDescEntry.Text?.Trim() ?? "No Description",
                Price = price,
                ImageUrl = _pickedImagePath ?? ProductImageEntry.Text?.Trim() ?? "dotnet_bot.png",
                Category = ProductCategoryEntry.Text?.Trim() ?? "General",
                SellerId = seller.Id
            };

            await _db.AddProduct(newProd);
            MyProducts.Add(newProd); // Adds to the visual list so you can select it later
            ClearEntries();
            _pickedImagePath = null;
            ProductImagePreview.IsVisible = false;

            await DisplayAlert("Success", $"{newProd.Name} listed!", "OK");
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
            selectedProduct.Name = ProductNameEntry.Text.Trim();
            selectedProduct.Description = ProductDescEntry.Text.Trim();
            selectedProduct.Price = newPrice;
            selectedProduct.ImageUrl = ProductImageEntry.Text.Trim();
            selectedProduct.Category = ProductCategoryEntry.Text.Trim();

            await _db.UpdateAsync(selectedProduct);

            var index = MyProducts.IndexOf(selectedProduct);
            if (index >= 0)
            {
                MyProducts.RemoveAt(index);
                MyProducts.Insert(index, selectedProduct); // Refresh the item in the list
            }

            ClearEntries();
            selectedProduct = null;
            await DisplayAlert("Success", "Product updated!", "OK");
        }
    }

    private async void OnDeleteProductClicked(object sender, EventArgs e)
    {
        if (selectedProduct == null)
        {
            await DisplayAlert("Rainy Day", "Please select a product from the list first!", "OK");
            return;
        }
        bool confirm = await DisplayAlert(
            "Confirm Deletion",
            $"Are you sure you want to delete {selectedProduct.Name}?",
            "Yes",
            "No"
        );
        if (confirm)
        {
            await _db.DeleteAsync(selectedProduct);
            MyProducts.Remove(selectedProduct);
            ClearEntries();
            selectedProduct = null;
            await DisplayAlert("Deleted", "Product has been removed.", "OK");
        }
    }

    private void ClearEntries()
    {
        ProductNameEntry.Text = string.Empty;
        ProductDescEntry.Text = string.Empty;
        ProductPriceEntry.Text = string.Empty;
        ProductImageEntry.Text = string.Empty;
        ProductCategoryEntry.Text = string.Empty;
        _pickedImagePath = null;
        ProductImagePreview.IsVisible = false;
    }

    private async void OnBackToStoreClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select a product image"
            });
            if (result != null)
            {
                _pickedImagePath = result.FullPath;
                ProductImageEntry.Text = _pickedImagePath; // Show the path in the entry for now

                ProductImagePreview.Source = ImageSource.FromFile(_pickedImagePath);
                ProductImagePreview.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
        }
    }
}