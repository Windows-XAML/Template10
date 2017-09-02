using System;
using System.Collections.Generic;
using System.Globalization;

namespace Template10.Services.Resources
{
    public class ResourceService : IResourceService
    {
        private ResourceHelper _helper;
        protected ResourceHelper Helper => _helper ?? (_helper = new ResourceHelper());

        public IReadOnlyList<string> SupportedLanguages => Helper.SupportedLanguages;

        public string CurrentLanguage
        {
            get => Helper.CurrentLanguage;
            set => Helper.CurrentLanguage = value;
        }

        public string GetLocalizedString(string key) => Helper.GetLocalizedStringAsync(key);

        public bool TryGetLocalizedString(string key, out string value)
        {
            try
            {
                value = Helper.GetLocalizedStringAsync(key);
                return true;
            }
            catch (Exception)
            {
                value = default(string);
                return false;
            }
        }

        public string GetLocalizedString(Uri uri) => Helper.GetLocalizedStringAsync(uri);

        public void SetLocale(string language)
        {
            Helper.SetLocale(language);
        }

        public void SetLocale(CultureInfo culture)
        {
            Helper.SetLocale(culture);
        }
    }
}
