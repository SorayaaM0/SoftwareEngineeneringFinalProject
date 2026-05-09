using StoreApp.Models;
using StoreApp.src.Services;
using System.Threading.Tasks;
namespace StoreApp.Views;

public partial class ProductDetailPage : ContentPage
{
	private readonly DatabaseService _db;
    private Product _product;
    private string _selectedSize = "";
    private bool _sidebarOpen = false;
    public ProductDetailPage(Product product, DatabaseService db)
	{
		InitializeComponent();
        _db = db;
		_product = product;

        ProductImage.Source = _product.ImageUrl;
		ProductName.Text = _product.Name;
		ProductPrice.Text = $"${_product.Price:F2}";
		ProductDescription.Text = _product.Description;
        ProductCategory.Text = _product.Category?.ToUpper();

        _ = CheckWishlistStatusAsync();
        _ = LoadSimilarProductAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateSidebar();
    }

    private async Task CheckWishlistStatusAsync()
    {
        if (!UserSession.IsLoggedIn) return;
        var existing = await _db.GetWishlistEntryAsync(
            UserSession.CurrentUser.Id, _product.ProductId);
        if (existing != null)
        {
            WishlistButton.Text = "❤️ Saved";
            WishlistButton.TextColor = Colors.White;
        }
    }

    private async Task LoadSimilarProductAsync()
    {
        if (string.IsNullOrEmpty(_product.Category)) return;
        var allProducts = await _db.GetAllAsync<Product>();
        var similar = allProducts
            .Where(p => p.Category == _product.Category && p.ProductId != _product.ProductId)
            .ToList();
        SimilarProductsCollection.ItemsSource = similar;
        SimilarProductsLabel.Text = $"More in { _product.Category.ToUpper() }";
        SimilarProductsCollection.IsVisible = similar.Any();
    }

    private async void OnSimilarProductTapped(object sender, EventArgs e)
    {
        if (sender is Border border &&
            border.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tap &&
            tap.CommandParameter is Product product)
        {
            // Navigate to a fresh detail page for the tapped product
            await Navigation.PushAsync(new ProductDetailPage(product, _db));
        }
    }

    private async void OnSimilarAddToCartClicked(object sender, EventArgs e)
    {
        if (!UserSession.IsLoggedIn)
        {
            await DisplayAlert("Login Required", "Please log in first.", "OK");
            return;
        }

        var product = (Product)((Button)sender).CommandParameter;

        var existing = await _db.GetCartItemAsync(
            UserSession.CurrentUser.Id, product.ProductId);

        if (existing != null)
        {
            existing.Quantity += 1;
            await _db.UpdateAsync(existing);
        }
        else
        {
            await _db.AddCartItem(new CartItem
            {
                BuyerId = UserSession.CurrentUser.Id,
                ProductId = product.ProductId,
                Quantity = 1,
                Price = product.Price
            });
        }

        await DisplayAlert("Added", $"{product.Name} added to cart!", "OK");
    }

    private async void OnWishlistClicked(object sender, EventArgs e)
    {
        if (!UserSession.IsLoggedIn)
        {
            await DisplayAlert("Login Required", "Please log in first.", "OK");
            return;
        }

        var existing = await _db.GetWishlistEntryAsync(
            UserSession.CurrentUser.Id, _product.ProductId);

        if (existing != null)
        {
            await DisplayAlert("Wishlist", $"{_product.Name} is already in your wishlist.", "OK");
            return;
        }

        await _db.AddWishlist(new Wishlist
        {
            BuyerId = UserSession.CurrentUser.Id,  // tied to THIS user
            ProductId = _product.ProductId
        });

        WishlistButton.Text = "❤️  Saved";
        WishlistButton.TextColor = Colors.White;
        await DisplayAlert("Wishlist", $"{_product.Name} added to your wishlist!", "OK");
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

    private void OnSizeSelected(object sender, EventArgs e)
    {
        if (sender is not Button tappedBtn) return;

        _selectedSize = tappedBtn.Text;

        foreach (var btn in SizeContainer.Children.OfType<Button>())
        {
            btn.BackgroundColor = Colors.Transparent;
            btn.TextColor = Colors.White;
            btn.BorderColor = Colors.Gray;
        }

        tappedBtn.BackgroundColor = Application.Current.Resources["Secondary"] as Color;
        tappedBtn.TextColor = Colors.Black;
        tappedBtn.BorderColor = Application.Current.Resources["Secondary"] as Color;
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

        }
        else
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

    private void UpdateSidebar()
    {
        if (UserSession.IsLoggedIn)
        {
            SidebarUserLabel.Text = $"Hello, {UserSession.CurrentUser.Name}!";
            SidebarEmailLabel.Text = UserSession.CurrentUser.Email;
            SidebarLoginBtn.IsVisible = false;
            SidebarLogoutBtn.IsVisible = true;
        }
        else
        {
            SidebarUserLabel.Text = "Welcome, Guest!";
            SidebarEmailLabel.Text = "Please log in to access more features.";
            SidebarLoginBtn.IsVisible = true;
            SidebarLogoutBtn.IsVisible = false;
        }
    }
     private async void OnSidebarLoginClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PushAsync(new LoginPage(_db));
    }

    private async void OnSidebarLogoutClicked(object sender, EventArgs e)
    {
        
        await CloseSidebarAsync();
        UserSession.Logout();
        await DisplayAlert("Logged Out", "You have been logged out successfully.", "OK");
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PopToRootAsync();
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PushAsync(new CartPage(_db));
    }

    private async void OnCollectionsClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        await Navigation.PushAsync(new WishlistPage(_db));
    }

    private async void OnAccountClicked(object sender, EventArgs e)
    {
        await CloseSidebarAsync();
        if (UserSession.IsLoggedIn)
            await Navigation.PushAsync(new AccountPage(_db));
        else
            await Navigation.PushAsync(new LoginPage(_db));
    }

}