using System.Collections.Generic;
using System.Linq;
using StoreApp.Models;
using StoreApp.Views;
using StoreApp.Services;
namespace StoreApp.Views;

public partial class ProductPage : ContentPage
{
    //data storage
    private List<Product> _allProducts;

    //objects from Model classes
    private ShoppingCart _cart;
    private Wishlist _wishlist;

    //state management for filtering
    private string _currentCategory = "All";
    private string _currentSort = "None";

    public ProductPage()
    {
        InitializeComponent();

        _allProducts = new List<Product>
        {
            new Product(1, "Classic White T-Shirt", "Premium cotton.", 29.99, "shirt.jpg", "TOPS"),
            new Product(2, "Slim Fit Denim Jeans", "Modern fit jeans.", 79.99, "jeans.jpg", "BOTTOMS"),
            new Product(3, "Cozy Pullover Hoodie", "Soft fleece hoodie.", 59.99, "hoodie.jpg", "OUTERWEAR"),
            new Product(4, "Floral Summer Dress", "Breezy summer dress.", 69.99, "dress.jpg", "DRESSES")
        };

        Buyer dummyBuyer = new Buyer(1, "User", "user@email.com", "hash123", "123 Main St", "555-0199");
        _cart = new ShoppingCart(101);
        _wishlist = new Wishlist(201, dummyBuyer);

        ProductsCollection.ItemsSource = _allProducts;
    }

    //filtering logic
    //ensures multiple filters can be active at one time
    private void ApplyFilters(string? searchTerm = "")
    {
        //filter by category
        IEnumerable<Product> filtered = _allProducts;
        if (_currentCategory != "All")
            filtered = filtered.Where(p => p.category.Equals(_currentCategory, StringComparison.OrdinalIgnoreCase));

        //filter by search
        if (!string.IsNullOrWhiteSpace(searchTerm))
            filtered = filtered.Where(p => p.name.ToLower().Contains(searchTerm.ToLower()));

        //sort by price
        if (_currentSort == "LowToHigh")
            filtered = filtered.OrderBy(p => p.price);
        else if (_currentSort == "HighToLow")
            filtered = filtered.OrderByDescending(p => p.price);

        ProductsCollection.ItemsSource = filtered.ToList();
    }

    private void OnCategorySelected(object sender, EventArgs e)
    {
        if (sender is MenuFlyoutItem item)
        {
            _currentCategory = item.CommandParameter?.ToString() ?? "All";
            CategoryBtn.Text = $"Category: {_currentCategory}";
            ApplyFilters(ProductSearchEntry.Text);
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

    //uses Wishlist.addProduct and removeProduct methods
    private void OnWishlistClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var product = (Product)button.CommandParameter;
        product.isWishlisted = !product.isWishlisted;

        if (product.isWishlisted) _wishlist.addProduct(product);
        else _wishlist.removeProduct(product);

        button.Text = product.isWishlisted ? "❤️" : "♡";
    }

    //uses ShoppingCart.addItem method
    private void OnAddToCartClicked(object sender, EventArgs e)
    {
        var product = (Product)((Button)sender).CommandParameter;
        _cart.addItem(product, 1);
        DisplayAlert("Added", $"{product.name} added to cart. Total Items: {_cart.items.Count}", "OK");
    }

    //passes shared objects to the different pages
    private async void OnCartClicked(object sender, EventArgs e) => await Navigation.PushAsync(new CartPage(_cart));
    private async void OnCollectionsClicked(object sender, EventArgs e) => await Navigation.PushAsync(new WishlistPage(_wishlist));

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateAccountButton();
    }

    private void UpdateAccountButton()
    {
        if(UserSession.IsLoggedIn)
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
            await Navigation.PushAsync(new AccountPage(_cart));
        }
        else
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }

    private async void OnProductTapped(object sender, EventArgs e)
    {
       if (sender is Frame frame &&
           frame.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tap &&
           tap.CommandParameter is Product product)
        {
            await Navigation.PushAsync(new ProductDetailPage(product,_cart));
        }
    }
}