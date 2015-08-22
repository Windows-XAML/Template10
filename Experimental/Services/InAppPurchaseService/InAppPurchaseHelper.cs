namespace Template10.Services.InAppPurchaseService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Windows.ApplicationModel.Store;
    using Windows.Networking.Connectivity;
    using Windows.Storage;

    public class InAppPurchaseHelper
    {
        private LicenseInformation license;
        private ListingInformation listing;
        private ProductListing product;

        /// <param name="featureKey">The key of the feature we are licensing, ex. NoAds</param>
        /// <param name="simulate">true to load from an xml file - ie simulate a fake license, false otherwise.</param>
        public InAppPurchaseHelper(string featureKey, bool simulate = true)
        {
            Key = featureKey;
            Simulate = simulate;
            CanPurchase = false;
            Debug.WriteLine(string.Format("InAppPurchaseHelper({0}, {1})", featureKey, simulate));
        }

        #region properties

        public bool Simulate { get; private set; }
        public string Key { get; private set; }

        /// <summary>
        ///     The feature's descriptive name as it is seen by customers in the current market.
        /// </summary>
        public string Name
        {
            get
            {
                try { return product.Name; }
                catch { return string.Empty; }
            }
        }

        /// <summary>
        ///     Returns true if the feature's license is active, and otherwise false. 
        ///     This property may return false if the license is missing, expired, or revoked.
        /// </summary>
        public bool IsPurchased
        {
            get
            {
                try { return license.ProductLicenses[Key].IsActive; }
                catch (Exception e)
                {
                    Debug.WriteLine(string.Format("IsPurchased [{0}, {1}, {2}]", Key, Simulate, e.Message));
                    return false;
                }
            }
        }

        // indicates that there are no errors
        public bool CanPurchase { get; private set; }

        public bool IsExpired { get { return Expires < DateTime.Now; } }

        /// <summary>
        ///     The date and time that the product's license expires.
        /// </summary>
        /// <remarks>
        ///     a valid date value == The license is active and expires on this date. 
        ///     This occurs during the product's lifetime (the time the product can be used).
        ///     maxDate == The license is active and never expires. This occurs if the product's lifetime is forever.
        ///     minDate == The license is not active.
        /// </remarks>
        public DateTime Expires
        {
            get
            {
                try { return license.ExpirationDate.DateTime; }
                catch (Exception e)
                {
                    Debug.WriteLine(string.Format("Expires [{0}, {1}, {2}]", Key, Simulate, e.Message));
                    return DateTime.MinValue;
                }
            }
        }
        /// <summary>
        ///     The Product ID is the string that the app uses to identify the in-app offer. 
        ///     The Product ID is entered in the Advance features page of the Dashboard where 
        ///     it is associated with the description, price tier and lifetime. 
        ///     The same Product ID is used in the app to get info about the product 
        ///     or feature that is enabled when the customer buys it through an in-app purchase.
        /// </summary>
        public string ProductId
        {
            get { return Key; }
        }

        /// <summary>
        ///     The app's purchase price with the appropriate formatting for the current market.
        /// </summary>
        public string FormattedPrice
        {
            get
            {
                try { return product.FormattedPrice; }
                catch (Exception e)
                {
                    Debug.WriteLine(string.Format("FormattedPrice [{0}, {1}, {2}]", Key, Simulate, e.Message));
                    return string.Empty;
                }
            }
        }

        public static bool HasInternetAccess
        {
            get
            {
                try
                {
                    var profile = NetworkInformation.GetInternetConnectionProfile();
                    if (profile == null)
                        return false;
                    return profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(string.Format("HasInternetAccess [{0}]", e.Message));
                    return false;
                }
            }
        }

        #endregion

        #region methods

        public async Task<InAppPurchaseHelper> SetupAsync()
        {
            // license
            if (Simulate)
            {
#if DEBUG
                await SetupSimulation();
                license = CurrentAppSimulator.LicenseInformation;
                listing = await CurrentAppSimulator.LoadListingInformationAsync();
                CanPurchase = true;
#endif
            }
            else
            {
                try
                {
                    license = CurrentApp.LicenseInformation;
                    listing = await CurrentApp.LoadListingInformationAsync();
                    CanPurchase = true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(string.Format("Setup/License [{0}, {1}, {2}]", Key, Simulate, e.Message));
                    CanPurchase = false;
                }
            }

            if (!CanPurchase)
                return this; // :(

            try
            {
                // test setup
                license.LicenseChanged += () => { RaiseLicenseChanged(IsPurchased); };
                IReadOnlyDictionary<string, ProductLicense> licenses = license.ProductLicenses;
                if (!licenses.Any(x => x.Key.Equals(Key, StringComparison.CurrentCultureIgnoreCase)))
                    throw new KeyNotFoundException(Key);

                IReadOnlyDictionary<string, ProductListing> products = listing.ProductListings;
                if (!products.Any(x => x.Key.Equals(Key, StringComparison.CurrentCultureIgnoreCase)))
                    throw new KeyNotFoundException(Key);

                // product
                product = products[Key];
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("Setup/Tests [{0}, {1}, {2}]", Key, Simulate, e.Message));
                CanPurchase = false;
            }
            return this;
        }

        /// <summary>
        ///     Proceeds with purchase
        /// </summary>
        /// <returns>Successfully purchased (no errors)</returns>
        public async Task<bool> Purchase()
        {
            if (!CanPurchase)
                return false;

            //If this feature is already active, exit on out.
            if (IsPurchased)
            {
                //Already showing as active
                return RaiseLicenseChanged(true);
            }
            try
            {
                if (Simulate)
                {
#if DEBUG
                    await CurrentAppSimulator.RequestProductPurchaseAsync(Key);
#endif
                }
                else
                {
                    // http://msdn.microsoft.com/en-us/library/windows/apps/Hh967814.aspx
                    // This method returns a success value even if:
                    //  1. There's no network connection available.
                    //  2. The user cancels out of the dialog.
                    //  3. The user's authentication fails.
                    //  4. Product purchase is success.
                    await CurrentApp.RequestProductPurchaseAsync(Key);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("Purchase [{0}, {1}, {2}]", Key, Simulate, e.Message));
                Debugger.Break();
            }

            return RaiseLicenseChanged(IsPurchased);
        }

        /// <summary>
        /// This method generates the XML necessary to let the simulator work
        /// </summary>
        private async Task SetupSimulation()
        {
            var document = new XDocument(
                new XDeclaration("1.0", "utf-16", "yes"),
                new XComment("SimulatedProduct"),
                new XElement("CurrentApp",
                        new XElement("ListingInformation",
                            new XElement("App",
                                new XElement("AppId", Guid.Empty.ToString()),
                                new XElement("LinkUri", "http://apps.microsoft.com/webpdp/app/" + Guid.Empty.ToString()),
                                new XElement("CurrentMarket", "en-us"),
                                new XElement("AgeRating", "3"),
                                new XElement("MarketData",
                                    new XAttribute(XNamespace.Xml + "lang", (XNamespace)"en-us"),
                                    new XElement("Name", "App Name"),
                                    new XElement("Description", "App Description"),
                                    new XElement("Price", "5.99"),
                                    new XElement("CurrencySymbol", "$"),
                                    new XElement("CurrencyCode", "USD")
                            ) // App.MarketData
                        ), // App
                            new XElement("Product",
                                new XAttribute("ProductId", Key),
                                new XElement("MarketData",
                                    new XAttribute(XNamespace.Xml + "lang", (XNamespace)"en-us"),
                                    new XElement("Name", Key),
                                    new XElement("Price", "1.49"),
                                    new XElement("CurrencySymbol", "$"),
                                    new XElement("CurrencyCode", "USD")
                            ) // Product.MarketData
                        ) // Product
                    ), // ListingInformation
                        new XElement("LicenseInformation",
                            new XElement("App",
                                // would be true when:
                                // 1. Is trial
                                // 2. Is not free AND is purchased
                                // 3. Is free
                                new XElement("IsActive", true),
                                new XElement("IsTrial", false)
                        ), // App
                            new XElement("Product",
                                new XAttribute("ProductId", Key),
                                new XElement("IsActive", false),
                                new XElement("ExpirationDate", DateTimeOffset.Now.AddDays(1).ToString("o"))
                        ) // Product
                    ) // LicenseInformation
                ) // CurrentApp
            );

            // save to temporary folder
            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            var simulatedProduct = await folder.CreateFileAsync(System.IO.Path.GetRandomFileName());
            // using System.IO;
            using (Stream stream = await simulatedProduct.OpenStreamForWriteAsync())
            {
                document.Save(stream, SaveOptions.None);
            }
#if DEBUG
            await CurrentAppSimulator.ReloadSimulatorAsync(simulatedProduct);
#endif
        }

        #endregion

        #region LicenseChanged event

        public event EventHandler LicenseChanged;

        protected bool RaiseLicenseChanged(bool value)
        {
            if (LicenseChanged != null)
                LicenseChanged(this, new LicenseChangedEventArgs { NewValue = value });
            return value;
        }

        public class LicenseChangedEventArgs : EventArgs
        {
            public bool NewValue { get; set; }
        }

        #endregion
    }

}
