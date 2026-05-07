using System.Collections.ObjectModel;
using System.Dynamic;
using StoreApp.src.Services;

namespace StoreApp.Views;

public partial class AdminProductModerationPage : ContentPage
{
    private ObservableCollection<dynamic> _reportedProducts;

    public AdminProductModerationPage()
    {
        InitializeComponent();
        LoadStubProducts();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Admin-only guard
        if (UserSession.CurrentUser is not Models.Admin)
        {
            await DisplayAlert(
                "Access Denied",
                "You are not authorized to view this page.",
                "OK"
            );
            await Navigation.PopAsync();
        }
    }

    private void LoadStubProducts()
    {
        _reportedProducts = new ObservableCollection<dynamic>
        {
            CreateProduct(
                "Inappropriate Shirt",
                "seller1@test.com",
                4
            ),
            CreateProduct(
                "Fake Brand Shoes",
                "seller2@test.com",
                2
            ),
            CreateProduct(
                "Offensive Artwork",
                "seller3@test.com",
                6
            )
        };

        ProductList.ItemsSource = _reportedProducts;
    }

    private dynamic CreateProduct(string name, string sellerEmail, int reports)
    {
        dynamic product = new ExpandoObject();
        product.Name = name;
        product.SellerEmail = sellerEmail;
        product.Reports = reports;
        return product;
    }

    private async void OnRemoveProductClicked(object sender, EventArgs e)
    {
        dynamic product = ((Button)sender).CommandParameter;

        bool confirm = await DisplayAlert(
            "Remove Product",
            $"Remove product:\n{product.Name}?",
            "Remove",
            "Cancel"
        );

        if (confirm)
        {
            _reportedProducts.Remove(product);

            await DisplayAlert(
                "Product Removed",
                $"{product.Name} has been removed.",
                "OK"
            );
        }
    }

    private async void OnRemoveSellerClicked(object sender, EventArgs e)
    {
        dynamic product = ((Button)sender).CommandParameter;

        bool confirm = await DisplayAlert(
            "Remove Seller Listings",
            $"Remove all listings from:\n{product.SellerEmail}?",
            "Remove",
            "Cancel"
        );

        if (confirm)
        {
            // Remove all products from this seller
            var toRemove = _reportedProducts
                .Where(p => p.SellerEmail == product.SellerEmail)
                .ToList();

            foreach (var item in toRemove)
                _reportedProducts.Remove(item);

            await DisplayAlert(
                "Seller Listings Removed",
                $"All listings from {product.SellerEmail} removed.",
                "OK"
            );
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}