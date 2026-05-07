using System.Collections.ObjectModel;
using System.Dynamic;
using StoreApp.src.Services;

namespace StoreApp.Views;

public partial class AdminSellerModerationPage : ContentPage
{
	private ObservableCollection<dynamic> _reportedSellers;
	public AdminSellerModerationPage()
	{ 
		InitializeComponent();
		LoadStubSellers();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if(UserSession.CurrentUser?.UserType != "Admin")
		{
			await DisplayAlert(
				"Access Denied",
				"You are not authorized to view this page",
				"OK");
			await Navigation.PopAsync();
		}
	}

	private void LoadStubSellers()
	{
		_reportedSellers = new ObservableCollection<dynamic>
		{
			CreateSeller("Seller One", "seller1@testmail.com",3),
			CreateSeller("Seller Two", "seller2@testmail.com",1),
			CreateSeller("Seller Three", "seller3@testmail.com",7),
			CreateSeller("Seller Four", "seller4@testmail.com",5),
		};
		SellerList.ItemsSource = _reportedSellers;
	}

	private dynamic CreateSeller(string name, string email, int reports)
	{
		dynamic seller = new ExpandoObject();
		seller.Name = name;
		seller.Email = email;
		seller.Reports = reports;
		return seller;
	}

    private async void OnRemoveListingsClicked(object sender, EventArgs e)
	{
        var seller = ((Button)sender).CommandParameter;
        dynamic dynSeller = seller;

		await DisplayAlert(
			"Listings Removed",
			$"All listings for {dynSeller.Email} would be removed",
			"OK");
    }


    private async void OnBanSellerClicked(object sender, EventArgs e)
	{
		var seller = ((Button)sender).CommandParameter;
		dynamic dynSeller = seller;
		bool confirm = await DisplayAlert(
			"Confirm Ban",
			$"Ban seller with email:\n{dynSeller.Email}?",
			"Ban",
			"Cancel");

		if (confirm)
		{
			_reportedSellers.Remove(seller);

			await DisplayAlert(
				"Seller Banned",
				$"{dynSeller.Email} has been banned",
				"OK"
				);
		}
	}

	private async void OnBackClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}