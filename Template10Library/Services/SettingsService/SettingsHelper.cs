using static Template10.Services.SettingsService.Settings;

namespace Template10.Services.SettingsService
{
	// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
	// according to suggestions: https://github.com/Windows-XAML/Template10/pull/50
	public class SettingsHelper
    {
        public Settings Container(SettingsStrategy strategy)
        {
            return (strategy == SettingsStrategy.Local)
                ? Local
                : Roaming;
        }

        public bool Exists(string key, SettingsStrategy strategy = SettingsStrategy.Local)
        {
            var settings = Container(strategy);
            return settings.Exists(key);
        }

        public void Remove(string key, SettingsStrategy strategy = SettingsStrategy.Local)
        {
            var settings = Container(strategy);
            if (settings.Exists(key))
                settings.Remove(key);
        }

        public void Write<T>(string key, T value, SettingsStrategy strategy = SettingsStrategy.Local)
        {
            var settings = Container(strategy);
			settings.Write(key, value);
		}

        public T Read<T>(string key, T otherwise, SettingsStrategy strategy = SettingsStrategy.Local)
        {
            var settings = Container(strategy);
			return settings.Read(key, otherwise);
        }

        public enum SettingsStrategy { Local, Roam, Temp }
    }
}
