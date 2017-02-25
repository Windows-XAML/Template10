namespace Template10.Services.SettingsService
{
    public interface ISettingsHelper
    {
        ISettingsService Container(SettingsStrategies strategy);
        bool Exists(string key, SettingsStrategies strategy = SettingsStrategies.Local);
        T Read<T>(string key, T otherwise, SettingsStrategies strategy = SettingsStrategies.Local);
        void Remove(string key, SettingsStrategies strategy = SettingsStrategies.Local);
        void Write<T>(string key, T value, SettingsStrategies strategy = SettingsStrategies.Local);
    }
}