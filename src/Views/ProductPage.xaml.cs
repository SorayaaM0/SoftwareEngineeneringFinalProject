using System.Collections.Generic;
using System.Linq;
using StoreApp.Models;
using StoreApp.src.Services;
using System.Threading.Tasks;
namespace StoreApp.Views;

public partial class ProductPage : ContentPage
{
    //data storage
    private List<Product> _allProducts;
    private bool _sidebarOpen = false;


    //state management for filtering
    private string _currentCategory = "All";
    private string _currentSort = "None";

    private readonly DatabaseService _db;

    public ProductPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //fetch products from database
        await LoadProductsAsync();
        UpdateAccountButton();
        System.Diagnostics.Debug.WriteLine(
            Path.Combine(FileSystem.AppDataDirectory, "storeapp.db")
         );
    }

    private async Task LoadProductsAsync()
    {
        _allProducts = await _db.GetAllAsync<Product>();
        if (UserSession.IsLoggedIn)
        {
            var wishlistEntries = await _db.GetWishlistAsync(UserSession.CurrentUser.Id);
            var wishlistIds = wishlistEntries.Select(w=>w.ProductId).ToHashSet();

            foreach (var prod in _allProducts)
            {
                prod.IsWishlisted = wishlistIds.Contains(prod.ProductId);
            }
        }
        ApplyFilters();
    }

    private void ApplyFilters(string? searchTerm = "")
    {
        //filter by category
        IEnumerable<Product> filtered = _allProducts ?? new List<Product>();

        if (_currentCategory != "All")
            filtered = filtered.Where(p => p.Category.Equals(_currentCategory, StringComparison.OrdinalIgnoreCase));
        //filter by search
        if (!string.IsNullOrWhiteSpace(searchTerm))
            filtered = filtered.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()));
        //sort by price
        if (_currentSort == "LowToHigh")
            filtered = filtered.OrderBy(p => p.Price);
        else if (_currentSort == "HighToLow")
            filtered = filtered.OrderByDescending(p => p.Price);

        ProductsCollection.ItemsSource = filtered.ToList();
    }

    private async void OnCategorySelected(object sender, EventArgs e)
    {
        string category = "All";
        if (sender is MenuFlyoutItem item)
        {
            category = item.CommandParameter?.ToString() ?? "All";
        } else if (sender is Button btn)
        {
            category = btn.CommandParameter?.ToString() ?? "All";
        }
        _currentCategory = category;
        CategoryBtn.Text = $"Category: {category}";
        ApplyFilters(ProductSearchEntry.Text);

        if (_sidebarOpen)
        {
            await CloseSidebarAsync();
        }

    }

    private void OnSortSelected(object sender, EventArgs e)
    {
        if (sender is MenuFlyoutItem item)
        {
            _currentSort = item.CommandParameter?.ToString() ?? "None";
            SortBtn.Text = item.Text;
            ApplyFilters(ProductSearchEntry.Text);
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e) => ApplyFilters(e.NewTextValue);

    private async void OnWishlistClicked(object sender, EventArgs e)
    {
        if (!UserSession.IsLoggedIn)
        {
            bool login = await DisplayAlert("Not Logged In",
                "Please log in to save items.", "Login", "Cancel");
            if (login) await Navigation.PushAsync(new LoginPage(_db));
            return;
        }

        var button = (Button)sender;
        var product = (Product)button.CommandParameter;

        var entry = await _db.GetWishlistEntryAsync(
            UserSession.CurrentUser.Id, product.ProductId);

        if (entry != null)
        {
            // Already wishlisted — remove it
            await _db.DeleteAsync(entry);
            product.IsWishlisted = false;
            button.Text = "♡";
        }
        else
        {
            // Not wishlisted — add it
            await _db.AddWishlist(new Wishlist
            {
                ProductId = product.ProductId,
                BuyerId = UserSession.CurrentUser.Id
            });
            product.IsWishlisted = true;
            button.Text = "❤️";
        }

        RefreshProductList();
    }

    private void RefreshProductList()
    {
        // Reassigning ItemsSource forces CollectionView to re-render
        var current = ProductsCollection.ItemsSource;
        ProductsCollection.ItemsSource = null;
        ProductsCollection.ItemsSource = current;
    }

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        var product = (Product)((Button)sender).CommandParameter;
        if (!UserSession.IsLoggedIn)
        {
            bool login = await DisplayAlert("Not Logged In", "Please log in to add items to your cart.", "Login", "Cancel");
            if (login)
            {
                await Navigation.PushAsync(new LoginPage(_db));
            }
            return;
        }

        var existing = await _db.GetCartItemAsync(UserSession.CurrentUser.Id, product.ProductId);
        if (existing != null)
        {
            existing.Quantity += 1;
            await _db.UpdateAsync(existing);

            

        }
        else
        {
            var newItem = new CartItem
            {
                BuyerId = UserSession.CurrentUser.Id,
                ProductId = product.ProductId,
                Quantity = 1,
                Price = product.Price
            };
            await _db.AddCartItem(newItem);
            
           
        }
        await DisplayAlert("Added to Cart", $"{product.Name} has been added to your cart.", "OK");
    }

    private async void OnCartClicked(object sender, EventArgs e) => await Navigation.PushAsync(new CartPage(_db));

    private async void OnCollectionsClicked(object sender, EventArgs e)
    {
        if (!UserSession.IsLoggedIn)
        {
            bool login = await DisplayAlert("Not Logged In", "Please log in to save items.", "Login", "Cancel");
            if (login) await Navigation.PushAsync(new LoginPage(_db));
            return;
        } else
        {

            await Navigation.PushAsync(new WishlistPage(_db));
        }



    }

    private void UpdateAccountButton()
    {
        if (UserSession.IsLoggedIn)
        {
            AccountButton.Text = $"My Account";
            AccountButton.IsEnabled = true;
        }
        else
        {
            AccountButton.Text = "Login / Register";
            AccountButton.IsEnabled = true;
        }
    }

    private async void OnAccountClicked(object sender, EventArgs e)
    {
        if (UserSession.IsLoggedIn)
        {
            await Navigation.PushAsync(new AccountPage(_db));
        }
        else
        {
            await Navigation.PushAsync(new LoginPage(_db));
        }
    }

    private async void OnProductTapped(object sender, EventArgs e)
    {
        if (sender is Border border &&
            border.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tap &&
            tap.CommandParameter is Product product)
        {
            await Navigation.PushAsync(new ProductDetailPage(product, _db));
        }
    }

    private async void OnMenuClicked(object sender, EventArgs e) => await OpenSidebarAsync();

    private async void OnOverlayTapped(object sender, EventArgs e) => await CloseSidebarAsync();

    private async void OnCloseSidebarClicked(object sender, EventArgs e) => await CloseSidebarAsync();

    private async Task OpenSidebarAsync()
    {
        if (UserSession.IsLoggedIn)
        {
            SidebarUserLabel.Text = $"Hello, {UserSession.CurrentUser.Name}!";
            SidebarEmailLabel.Text = UserSession.CurrentUser.Email;
            SidebarLoginBtn.IsVisible = false;
            SidebarLogoutBtn.IsVisible = true;

        } else
        {
            SidebarUserLabel.Text = "Welcome, Guest!";
            SidebarEmailLabel.Text = "Please log in to access more features.";
            SidebarLoginBtn.IsVisible = true;
            SidebarLogoutBtn.IsVisible = false;
        }
        SidebarOverlay.IsVisible = true;
        _sidebarOpen = true;

        await Task.WhenAll(
            SidebarOverlay.FadeTo(0.5, 250),
            SidebarPanel.TranslateTo(0, 0, 250, Easing.CubicOut)
        );
    }

    private async Task CloseSidebarAsync()
    {
        _sidebarOpen = false;

        await Task.WhenAll(
            SidebarOverlay.FadeTo(0, 200),
            SidebarPanel.TranslateTo(300, 0, 200, Easing.CubicIn)
        );

        SidebarOverlay.IsVisible = false;
    }

    private async void OnSidebarLoginClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PushAsync(new LoginPage(_db));
    }

    private async void OnSidebarLogoutClicked(object sender, EventArgs e)
    {
        UserSession.Logout();
        UpdateAccountButton();
        await CloseSidebarAsync();
        await DisplayAlert("Logged Out", "You have been logged out successfully.", "OK");
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PopToRootAsync();
    }



}