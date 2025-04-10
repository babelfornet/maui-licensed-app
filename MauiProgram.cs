using Babel.Licensing;
using Microsoft.Extensions.Logging;
using MauiLicApp.Licensing;

namespace MauiLicApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		// Configure BabelLicensing service URL for HTTPS requests
		// Change to your actual licensing server URL
		// When running on an Android device or iOS emulator, 
		// "localhost" refers to the Android device itself, 
		// not the host machine where the license server is running. 
		// Set the URL with the appropriate address to reach your host machine.
		string babelHttpServiceUrl = "https://192.168.1.11:5455";

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseBabelLicensing(config => {
				config.ServiceUrl = babelHttpServiceUrl;
			})
			.UseBabelReporting(config => {
				config.ServiceUrl = babelHttpServiceUrl;
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register the LicensingService for dependency injection
		builder.Services.AddSingleton<LicensingService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
