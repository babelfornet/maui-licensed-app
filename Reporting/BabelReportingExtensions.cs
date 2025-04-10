using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.LifecycleEvents;

using Babel.Licensing;
using MauiLicApp.Licensing;

namespace MauiLicApp
{
    public static class BabelReportingExtensions
    {
        /// <summary>
        /// Registers the BabelReporting client for dependency injection throughout the application.
        /// </summary>
        /// <param name="builder">The MauiAppBuilder instance.</param>
        /// <param name="configureClient">Optional action to configure the BabelReporting client.</param>
        /// <returns>The MauiAppBuilder instance for method chaining.</returns>
        public static MauiAppBuilder UseBabelReporting(this MauiAppBuilder builder, Action<BabelReportingConfiguration>? configureClient = null)
        {
            // Create the BabelReporting configuration
            var config = new BabelReportingConfiguration {
                ClientId = AppInfo.Current.Name,                    
            };
            
            // Use HTTP  
            config.UseHttp(http => {
                http.Timeout = TimeSpan.FromSeconds(3);
                http.Handler = new HttpClientHandler() {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
            });

            // Allow custom configuration
            configureClient?.Invoke(config);

            // Create and register the BabelReporting client
            builder.Services.AddSingleton<BabelReporting>(serviceProvider =>
            {
                // Create the client instance
                var reporting = new BabelReporting(config);

                // Set up the BeforeSendReport event handler with default implementation
                reporting.BeforeSendReport += (s, e) => {
                    // Add basic environment information to all reports by default
                    e.Report.Properties.Add("app_version", AppInfo.Current.VersionString);
                    e.Report.Properties.Add("device_platform", DeviceInfo.Current.Platform.ToString());
                    e.Report.Properties.Add("device_model", DeviceInfo.Current.Model);
                };

                return reporting;
            });

            return builder;
        }
        
        /// <summary>
        /// Reports an unhandled exception asynchronously.
        /// </summary>
        /// <param name="ex"> The exception to report.</param>
        /// <remarks>
        /// This method is designed to be called from the AppDomain.UnhandledException event handler.
        /// It handles the exception reporting asynchronously to avoid blocking the main thread.
        /// It also prevents reentrant calls to avoid potential deadlocks in exception handlers.
        /// </remarks>
        public static void ReportUnhandledException(this Exception ex)
        {
            try 
            {
                // Don't use Wait() as it can cause deadlocks in exception handler context
                // Instead just fire and forget the report task
                var task = Task.Run(async () => 
                {
                    try 
                    {
                        await ex.SendReportAsync();
                    }
                    catch (Exception innerEx)
                    {
                        Debug.WriteLine($"Failed to report unhandled exception: {innerEx.Message}");
                    }
                });

                task.Wait(TimeSpan.FromSeconds(5)); // Wait for a short time to allow reporting
            }
            catch (Exception reportEx)
            {
                Debug.WriteLine($"Failed to report unhandled exception: {reportEx.Message}");
            }
        }

        // Flag to detect whether we're in an unhandled exception handler
        private static bool _isReportingException = false;

        public static async Task SendReportAsync(this Exception ex)
        {
            // Prevent reentrant calls to avoid potential deadlocks in exception handlers
            if (_isReportingException)
            {
                Debug.WriteLine("Already reporting an exception, skipping to prevent reentrance");
                return;
            }
            
            try
            {
                _isReportingException = true;
                
                // Get application services safely
                var services = IPlatformApplication.Current?.Services;
                if (services == null)
                {
                    Debug.WriteLine("Cannot report exception: Application services unavailable");
                    return;
                }
                
                // Get required services from DI container
                var reporting = services.GetRequiredService<BabelReporting>();
                var licensingService = services.GetRequiredService<LicensingService>();
                
                string? userKey = licensingService.UserKey;
                if (string.IsNullOrEmpty(userKey))
                    return;
                    
                await reporting.SendExceptionReportAsync(userKey, ex);
                Debug.WriteLine($"Exception reported: {ex.Message}");
            }
            catch (Exception reportEx)
            {
                // Log failure to report exception (avoid infinite loop)
                Debug.WriteLine($"Failed to report exception: {reportEx.Message}");
            }
            finally
            {
                _isReportingException = false;
            }
        }        
    }
}
