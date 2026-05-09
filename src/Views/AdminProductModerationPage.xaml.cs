using StoreApp.Models;
using StoreApp.src.Services;
using StoreApp.src.Models;
using System.Collections.ObjectModel;

namespace StoreApp.Views;

public partial class AdminProductModerationPage : ContentPage
{
    private readonly DatabaseService _db;
    private ObservableCollection<ReportedProductDisplay> _reportedProducts = new();

    public AdminProductModerationPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        ProductList.ItemsSource = _reportedProducts;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (UserSession.CurrentUser?.UserType != "Admin")
        {
            await DisplayAlert("Access Denied",
                "You are not authorized to view this page.", "OK");
            await Navigation.PopAsync();
            return;
        }

        await LoadReportedProductsAsync();
    }

    private async Task LoadReportedProductsAsync()
    {
        var reports = await _db.GetUnresolvedReportsAsync();
        _reportedProducts.Clear();

        // Group by product so we show report count
        var grouped = reports.GroupBy(r => r.ProductId);

        foreach (var group in grouped)
        {
            var product = await _db.GetByIdAsync<Product>(group.Key);
            if (product == null) continue;

            var seller = await _db.GetByIdAsync<User>(product.SellerId);

            // Show the most recent report for this product
            var latestReport = group.OrderByDescending(r => r.ReportedAt).First();

            _reportedProducts.Add(new ReportedProductDisplay
            {
                Report = latestReport,
                Product = product,
                Seller = seller,
                ReportCount = group.Count()
            });
        }
    }

    private async void OnRemoveProductClicked(object sender, EventArgs e)
    {
        var display = (ReportedProductDisplay)((Button)sender).CommandParameter;

        bool confirm = await DisplayAlert("Remove Product",
            $"Permanently delete '{display.ProductName}'?", "Remove", "Cancel");
        if (!confirm) return;

        // Delete the product from DB
        await _db.DeleteAsync(display.Product);

        // Mark all reports for this product as resolved
        var reports = await _db.GetReportsForProductAsync(display.Product.ProductId);
        foreach (var report in reports)
        {
            report.IsResolved = true;
            await _db.UpdateAsync(report);
        }

        _reportedProducts.Remove(display);
        await DisplayAlert("Done", $"'{display.ProductName}' has been removed.", "OK");
    }

    private async void OnRemoveSellerClicked(object sender, EventArgs e)
    {
        var display = (ReportedProductDisplay)((Button)sender).CommandParameter;

        bool confirm = await DisplayAlert("Ban Seller",
            $"Remove all listings from {display.SellerEmail}?", "Remove", "Cancel");
        if (!confirm) return;

        // Get all products by this seller and delete them
        var sellerProducts = await _db.GetProductsBySellerAsync(display.Seller.Id);
        foreach (var product in sellerProducts)
            await _db.DeleteAsync(product);

        // Resolve all their reports
        foreach (var item in _reportedProducts
            .Where(r => r.Seller?.Id == display.Seller.Id).ToList())
        {
            _reportedProducts.Remove(item);
        }

        await DisplayAlert("Done",
            $"All listings from {display.SellerEmail} removed.", "OK");
    }

    private async void OnMarkResolvedClicked(object sender, EventArgs e)
    {
        var display = (ReportedProductDisplay)((Button)sender).CommandParameter;

        display.Report.IsResolved = true;
        await _db.UpdateAsync(display.Report);
        _reportedProducts.Remove(display);

        await DisplayAlert("Resolved",
            $"Report for '{display.ProductName}' marked as resolved.", "OK");
    }

    private async void OnBackClicked(object sender, EventArgs e)
        => await Navigation.PopAsync();
}