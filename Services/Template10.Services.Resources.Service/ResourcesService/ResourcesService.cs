using System;
using System.Collections.Generic;
using System.Globalization;

namespace Template10.Services.Resources
{
    public class ResourcesService : IResourcesService
    {
        private ResourcesHelper _helper;
        protected ResourcesHelper Helper => _helper ?? (_helper = new ResourcesHelper());

        public IReadOnlyList<string> SupportedLanguages => Helper.SupportedLanguages;

        public string CurrentLanguage
        {
            get => Helper.CurrentLanguage;
            set => Helper.CurrentLanguage = value;
        }

        public string GetLocalizedString(string key) => Helper.GetLocalizedStringAsync(key);


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
