using System;
using System.Collections.Generic;
using System.Globalization;

namespace Template10.Service.LocalizationService.Services.LocalizationService
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Set language/culture of application by passing language string
        /// </summary>
        /// <param name="language"> language (example: "en-US")</param>
        void SetLocale(string language);

        /// <summary>
        /// Set language/culture of application by passing CultureInfo object
        /// </summary>
        /// <param name="culture">CultureInfo object</param>
        void SetLocale(CultureInfo culture);

        /// <summary>
        /// Get a localized string by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        string GetLocalizedString(string key);

        /// <summary>
        /// Get a localized string by Uri
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        string GetLocalizedString(Uri uri);

        /// <summary>
        /// Supported languages
        /// </summary>
        IReadOnlyList<string> SupportedLanguages { get; }
        
        /// <summary>
        /// The current language of app
        /// </summary>
        string CurrentLanguage { get; set; }
    }
}
