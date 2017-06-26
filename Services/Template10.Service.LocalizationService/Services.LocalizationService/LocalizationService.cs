using System;
using System.Collections.Generic;
using System.Globalization;

namespace Template10.Service.LocalizationService.Services.LocalizationService
{
    public class LocalizationService : ILocalizationService
    {
        protected LocalizationHelper Helper { get; private set; }
        public LocalizationService() => Helper = new LocalizationHelper();

        public IReadOnlyList<string> SupportedLanguages => Helper.SupportedLanguages;
        public string CurrentLanguage { get => Helper.CurrentLanguage; set => Helper.CurrentLanguage = value; }

        public void SetLocale(string language) { Helper.SetLocale(language); }
        public void SetLocale(CultureInfo culture) { Helper.SetLocale(culture); }

        public string GetLocalizedString(string key) { return Helper.GetLocalizedString(key); }
        public string GetLocalizedString(Uri uri) { return Helper.GetLocalizedString(uri); }
    }
}
