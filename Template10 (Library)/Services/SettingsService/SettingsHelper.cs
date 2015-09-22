using static Template10.Services.SettingsService.SettingsService;

namespace Template10.Services.SettingsService
{
	// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
	// according to suggestions: https://github.com/Windows-XAML/Template10/pull/50
	public class SettingsHelper : ISettingsHelper
    {
        public SettingsService Container(SettingsStrategies strategy)
        {
            return (strategy == SettingsStrategies.Local)
                ? Local
                : Roaming;
        }

        public bool Exists(string key, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            var settings = Container(strategy);
            return settings.Exists(key);
        }

        public void Remove(string key, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            var settings = Container(strategy);
            if (settings.Exists(key))
                settings.Remove(key);
        }

        public void Write<T>(string key, T value, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            var settings = Container(strategy);
			settings.Write(key, value);
		}

        public T Read<T>(string key, T otherwise, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            var settings = Container(strategy);
			return settings.Read(key, otherwise);
        }

    }
}
