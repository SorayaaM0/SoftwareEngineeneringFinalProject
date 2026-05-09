using StoreApp.src.Services;
using StoreApp.Models;
namespace StoreApp.src.Views;
public partial class OrderHistoryPage : ContentPage
{
    private readonly DatabaseService _db;

    public OrderHistoryPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Use the logged-in user's ID to filter orders
        if (UserSession.CurrentUser != null)
        {
            var history = await _db.GetOrdersByBuyerAsync(UserSession.CurrentUser.Id);
            OrdersCollectionView.ItemsSource = history;
        }
    }
}