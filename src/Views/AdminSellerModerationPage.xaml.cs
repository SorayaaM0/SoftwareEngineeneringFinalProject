using System.Collections.ObjectModel;
using System.Dynamic;
using StoreApp.src.Services;
using StoreApp.src.Models;
namespace StoreApp.Views;

public partial class AdminSellerModerationPage : ContentPage
{
	private ObservableCollection<dynamic> _reportedSellers = new();
	private readonly DatabaseService _db;
	public AdminSellerModerationPage(DatabaseService db)
	{ 
		InitializeComponent();
		_db = db;
        SellerList.ItemsSource = _reportedSellers;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if(UserSession.CurrentUser?.UserType != "Admin")
		{
            await _db.LogSecurityEventAsync(
            "Unauthorized Admin Access",
            UserSession.CurrentUser?.Email ?? "Unknown",
            $"Attempted to access {GetType().Name}",
            UserSession.CurrentUser?.Id);

            await DisplayAlert(
				"Access Denied",
				"You are not authorized to view this page",
				"OK");
			await Navigation.PopAsync();
			return;
		}
		await LoadSellersAsync();
	}

    private async Task LoadSellersAsync()
    {
        // Get all sellers from DB
        var allSellers = await _db.GetAllSellersAsync();
        _reportedSellers.Clear();

        foreach (var seller in allSellers)
        {
            // Get their products and report count
            var products = await _db.GetProductsBySellerAsync(seller.Id);
            int reportCount = 0;

            foreach (var product in products)
            {
                var reports = await _db.GetReportsForProductAsync(product.ProductId);
                reportCount += reports.Count;
            }

            _reportedSellers.Add(new SellerDisplay
            {
                Seller = seller,
                ProductCount = products.Count,
                ReportCount = reportCount
            });
        }
    }

    private async void OnRemoveListingsClicked(object sender, EventArgs e)
    {
        var display = (SellerDisplay)((Button)sender).CommandParameter;

        bool confirm = await DisplayAlert("Remove Listings",
            $"Remove all products from {display.Email}?", "Remove", "Cancel");
        if (!confirm) return;

        var products = await _db.GetProductsBySellerAsync(display.Seller.Id);
        foreach (var product in products)
            await _db.DeleteAsync(product);

        // Log the action
        await _db.LogSecurityEventAsync(
            "Admin Action — Listings Removed",
            display.Email,
            $"Admin removed all {products.Count} listings from seller {display.Name}",
            display.Seller.Id);

        await DisplayAlert("Done",
            $"All listings from {display.Email} have been removed.", "OK");

        // Refresh
        await LoadSellersAsync();
    }

    private async void OnBanSellerClicked(object sender, EventArgs e)
    {
        var display = (SellerDisplay)((Button)sender).CommandParameter;

        bool confirm = await DisplayAlert("Ban Seller",
            $"Ban {display.Name} ({display.Email})?\nThis will remove all their listings.",
            "Ban", "Cancel");
        if (!confirm) return;

        // Remove all their products first
        var products = await _db.GetProductsBySellerAsync(display.Seller.Id);
        foreach (var product in products)
            await _db.DeleteAsync(product);

        // Set IsBanned flag
        display.Seller.IsBanned = true;
        await _db.UpdateAsync(display.Seller);

        // Log the ban
        await _db.LogSecurityEventAsync(
            "Admin Action — Seller Banned",
            display.Email,
            $"Admin banned seller {display.Name} and removed {products.Count} listings",
            display.Seller.Id);

        _reportedSellers.Remove(display);

        await DisplayAlert("Seller Banned",
            $"{display.Name} has been banned and their listings removed.", "OK");
    }

    private async void OnBackClicked(object sender, EventArgs e) => await Navigation.PopAsync();

}