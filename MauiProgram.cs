using Microsoft.Extensions.Logging;
using StoreApp.src.Services;
using StoreApp.src.Views;
using StoreApp.Views;
namespace StoreApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddSingleton<UserFactory>();
        builder.Services.AddTransient<ProductPage>();
		builder.Services.AddTransient<LoginPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
