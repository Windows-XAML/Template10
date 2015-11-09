using static Template10.Services.SettingsService.SettingsService;

namespace Template10.Services.SettingsService
{
    public class SettingsHelper : ISettingsHelper
    {
        public ISettingsService Container(SettingsStrategies strategy)
        {
            return (strategy == SettingsStrategies.Local) ? Local : Roaming;
        }

        public bool Exists(string key, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return true;
            return Container(strategy).Exists(key);
        }

        public void Remove(string key, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            if (Container(strategy).Exists(key))
                Container(strategy).Remove(key);
        }

        public void Write<T>(string key, T value, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            Container(strategy).Write(key, value);
        }

        public T Read<T>(string key, T otherwise, SettingsStrategies strategy = SettingsStrategies.Local)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return otherwise;
            return Container(strategy).Read(key, otherwise);
        }

    }
}
