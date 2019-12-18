using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;

namespace Template10.Services
{
    public class ResourceHelper
    {
        private readonly ResourceLoader _loader = ResourceLoader.GetForCurrentView();

        /// <summary>
        /// All supported languages as per String resource files in application
        /// </summary>
        public IReadOnlyList<string> SupportedLanguages => ApplicationLanguages.ManifestLanguages;

        #region CurrentLanguage
        private string _currentLanguage = CultureInfo.CurrentCulture.Name;
        /// <summary>
        /// Current language
        /// </summary>
        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (value != null && !_currentLanguage.Equals(value))
                {
                    SetLocale(value);
                }
            }
        }
        #endregion

        /// <summary>
        /// Get localized string from resource by key
        /// </summary>
        /// <param name="key">Resource key (example: "login", "login/color")</param>
        /// <returns></returns>
        public string GetLocalizedStringAsync(string key) => _loader.GetString(key);

        /// <summary>
        /// Get localized string from resource by uri
        /// </summary>
        /// <param name="uri">Resource uri</param>
        /// <returns></returns>
        public string GetLocalizedStringAsync(Uri uri) => _loader.GetStringForUri(uri);

        /// <summary>
        /// Set language/culture of application by passing language string
        /// </summary>
        /// <param name="language">language (example: "en-US")</param>
        public void SetLocale(string language)
        {
            _currentLanguage = language;
            ApplicationLanguages.PrimaryLanguageOverride = language;
            ResourceContext.GetForViewIndependentUse()?.Reset();
            ResourceContext.GetForCurrentView()?.Reset();
        }

        /// <summary>
        /// Set language/culture of application by passing CultureInfo object
        /// </summary>
        /// <param name="culture">CultureInfo object</param>
        public void SetLocale(CultureInfo culture)
        {
            SetLocale(culture.Name);
        }
    }
}