using Microsoft.Extensions.DependencyInjection;
using StoreApp.src.Services;
using StoreApp.Views;

namespace StoreApp;

public partial class App : Application
{
    public App(DatabaseService db)
    {
        InitializeComponent();
        MainPage = new NavigationPage(new ProductPage(db));
        
    }

}