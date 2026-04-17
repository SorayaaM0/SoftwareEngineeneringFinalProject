using Microsoft.Extensions.DependencyInjection;

namespace StoreApp;

public partial class App : Application
{
    public App()
{
    InitializeComponent();
    MainPage = new NavigationPage(new StoreApp.Views.ProductPage());
}

}