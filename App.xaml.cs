using Babel.Licensing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using MauiLicApp.Licensing;
using System.Threading.Tasks;

namespace MauiLicApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		
		// Set up global exception handling
		SetupExceptionHandling();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
	
	private void SetupExceptionHandling()
	{
		// Handle exceptions in the current AppDomain
		AppDomain.CurrentDomain.UnhandledException += (sender, args) => 
		{
			var ex = args.ExceptionObject as Exception;
			ex?.ReportUnhandledException();
		};
	}	
}
