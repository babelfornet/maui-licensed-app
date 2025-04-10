namespace MauiLicApp;

using Babel.Licensing;
using MauiLicApp.Licensing;
using System.Diagnostics;

public partial class MainPage : ContentPage
{
	private readonly LicensingService _licensingService;
	private readonly SystemInformationBase _systemInfo;

	public MainPage(LicensingService licensingService, SystemInformationBase systemInfo)
	{
		InitializeComponent();
		
		_licensingService = licensingService;
		_systemInfo = systemInfo;
		
		// Update the button text based on license status
		UpdateLicenseStatus();
	}
	
	private void UpdateLicenseStatus()
	{
		try
		{
			bool isLicensed = _licensingService.IsLicensed;
			ActivateBtn.Text = isLicensed 
				? "License Activated! Click to Validate" 
				: "Activate License";
			
			// Make DeactivateBtn and CrashBtn visible and enabled only if license is activated
			DeactivateBtn.IsVisible = isLicensed;
			DeactivateBtn.IsEnabled = isLicensed;
			CrashBtn.IsVisible = isLicensed;
			CrashBtn.IsEnabled = isLicensed;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error checking license status: {ex.Message}");
			ActivateBtn.Text = "Error checking license";
			DeactivateBtn.IsVisible = false;
			DeactivateBtn.IsEnabled = false;
		}
	}

	private async void OnActivateClicked(object sender, EventArgs e)
	{
		try
		{
			if (_licensingService.IsLicensed)
			{
				// If licensed, validate the license
				var license = await _licensingService.ValidateLicenseAsync();
				ActivateBtn.Text = $"License valid: {license.Id}";
				
				// Show system information
				await DisplayAlert("System Info", 
					$"Device: {_systemInfo.DeviceManufacturer} {_systemInfo.DeviceModel}\n" +
					$"OS: {_systemInfo.OperatingSystem} {_systemInfo.OsVersion}\n" +
					$"System: {_systemInfo.SystemName}\n" +
					$"UUID: {_systemInfo.SystemUuid}", 
					"OK");
			}
			else
			{
				// If not licensed, prompt for activation
				string userKey = await DisplayPromptAsync("License Activation", 
					"Enter your license key:", 
					accept: "Activate", 
					cancel: "Cancel");
				
				if (!string.IsNullOrEmpty(userKey))
				{
					try
					{
						await _licensingService.ActivateLicenseAsync(userKey);
						await DisplayAlert("Success", "License activated successfully!", "OK");
						UpdateLicenseStatus();
					}
					catch (Exception ex)
					{
						await DisplayAlert("Activation Failed", $"Could not activate license: {ex.Message}", "OK");
					}
				}
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"License operation failed: {ex.Message}", "OK");
		}

		SemanticScreenReader.Announce(ActivateBtn.Text);
	}

	private async void OnDeactivateClicked(object sender, EventArgs e)
	{
		try
		{
			await _licensingService.DeactivateLicenseAsync();
			await DisplayAlert("Success", "License deactivated successfully!", "OK");
			UpdateLicenseStatus();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Deactivation Failed", $"Could not deactivate license: {ex.Message}", "OK");
		}
		
		SemanticScreenReader.Announce(ActivateBtn.Text);
	}
	
	private void OnCrashClicked(object sender, EventArgs e)
	{
		// Deliberately throw an unhandled exception to trigger exception report generation
		// This button is only visible when license is activated		
		SemanticScreenReader.Announce("Generating exception report");
		
		throw new InvalidOperationException("Manually triggered exception for report generation");
	}
}
