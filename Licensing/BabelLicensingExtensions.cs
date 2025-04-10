using Babel.Licensing;
using MauiLicApp.Licensing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.LifecycleEvents;

namespace MauiLicApp
{
    public static class BabelLicensingExtensions
    {
        /// <summary>
        /// Registers the BabelLicensing client for dependency injection throughout the application.
        /// </summary>
        /// <param name="builder">The MauiAppBuilder instance.</param>
        /// <param name="configureClient">Optional action to configure the BabelLicensing client.</param>
        /// <returns>The MauiAppBuilder instance for method chaining.</returns>
        public static MauiAppBuilder UseBabelLicensing(this MauiAppBuilder builder, Action<BabelLicensingConfiguration>? configureClient = null)
        {
            // Register platform-specific system information service
            builder.Services.AddSingleton<SystemInformationBase>(serviceProvider =>
            {
                return LicenseServices.Current.GetService(typeof(ISystemInformation)) as SystemInformationBase ?? 
                throw new InvalidOperationException("SystemInformation service not registered.");
            });

            // Create the BabelLicensing configuration
            var config = new BabelLicensingConfiguration {
                // Set the default client ID
                ClientId = AppInfo.Current.Name,
                // Set the machine ID using system UUID and name
                MachineId = new HardwareId(HardwareComponents.SystemUuid | HardwareComponents.SystemName).ToMachineKey(),                
                // Set the signature provider with public key for license verification
                SignatureProvider = RSASignature.FromKeys("MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDE1VRiIdr6fiVZKve7NVgjIvGdRiRx0Mjjm+Yzf6tLbzFnxLs0fat5EoRcubxx0QQQDfydsJBE/fc7cwRWSrE2xK6X4Eb4W8O47pCMjqvTQZfDqQywEZJrLlxpp9hlKz6FDYX4SagrjmP1gdw8olo+n+IBz8ubkNxRhvycikxuDQIDAQAB")
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

            // Create and register the BabelLicensing client
            builder.Services.AddSingleton<BabelLicensing>(serviceProvider =>
            {
                // Create the client instance
                var client = new BabelLicensing(config);
                return client;
            });

            // Register the BabelLicenseProvider as a singleton
            builder.Services.AddSingleton<BabelServiceLicenseProvider>(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<BabelLicensing>();
                var provider = new BabelServiceLicenseProvider(client)
                {
                    // Refresh the license contacting every time
                    // You can set this to a specific interval if needed
                    // LicenseRefreshInterval = TimeSpan.FromDays(10)
                    // For testing purposes, set to zero to always check the license
                    // with the server.
                    // This is not recommended for production use
                    LicenseRefreshInterval = TimeSpan.Zero,
                };

                // Register the license provider with BabelLicenseManager
                BabelLicenseManager.RegisterLicenseProvider(typeof(BabelLicensingExtensions), provider);

                return provider;
            });

            return builder;
        }
    }
}
