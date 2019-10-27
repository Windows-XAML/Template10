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

        public string GetLocalizedString(string key, string fallback = null)
        {
            try
            {
                return Helper.GetLocalizedStringAsync(key);
            }
            catch (Exception)
            {
                if (fallback == null)
                {
                    throw;
                }
                return fallback;
            }
        }

        public bool TryGetLocalizedString(string key, out string value)
        {
            try
            {
                value = Helper.GetLocalizedStringAsync(key);
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception)
            {
                value = default(string);
                return false;
            }
        }

        public string GetLocalizedString(Uri uri, string fallback = null)
        {
            try
            {
                return Helper.GetLocalizedStringAsync(uri);
            }
            catch (Exception)
            {
                if (fallback == null)
                {
                    throw;
                }
                return fallback;
            }
        }

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
