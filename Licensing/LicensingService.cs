using Babel.Licensing;

namespace MauiLicApp.Licensing
{
    /// <summary>
    /// Service for handling licensing operations throughout the application.
    /// This service can be injected into any component that needs licensing functionality.
    /// </summary>
    public class LicensingService
    {
        private readonly BabelLicensing _client;
        private readonly BabelServiceLicenseProvider _licenseProvider;

        public LicensingService(BabelLicensing client, BabelServiceLicenseProvider licenseProvider)
        {
            _client = client;
            _licenseProvider = licenseProvider;
        }

        /// <summary>
        /// Gets the current BabelLicensing client.
        /// </summary>
        public BabelLicensing Client => _client;

        /// <summary>
        /// Gets whether the application is currently licensed.
        /// </summary>
        public bool IsLicensed => BabelLicenseManager.IsLicensed(typeof(BabelLicensingExtensions));

        /// <summary>
        /// Gets the current user key used for licensing.
        /// </summary>
        public string? UserKey => _licenseProvider.UserKey;

        /// <summary>
        /// Activates a license using the provided user key.
        /// </summary>
        /// <param name="userKey">The user key to activate.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ActivateLicenseAsync(string userKey)
        {
            await _client.ActivateLicenseAsync(userKey, typeof(BabelLicensingExtensions));
        }

        /// <summary>
        /// Deactivates the current license.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeactivateLicenseAsync()
        {
            if (!string.IsNullOrEmpty(UserKey))
            {
                await _client.DeactivateLicenseAsync(UserKey);
            }
        }

        /// <summary>
        /// Validates the current license.
        /// </summary>
        /// <returns>A task that resolves to the license object if valid.</returns>
        public async Task<ILicense> ValidateLicenseAsync()
        {
            return await BabelLicenseManager.ValidateAsync(typeof(BabelLicensingExtensions));
        }
    }
}
