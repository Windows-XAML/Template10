using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
    // this should not be made abstract
    public class SettingsServiceBase : ISettingsService
    {
        protected ApplicationDataContainer _container;
        protected SettingsStrategies _strategy;

        public SettingsServiceBase() : this(SettingsStrategies.Local, nameof(SettingsServiceBase), true)
        {
            // 
        }

        public SettingsServiceBase(SettingsStrategies strategy, string folderName, bool createFolderIfNotExists)
        {
            _strategy = strategy;
            Helper = new SettingsHelper();

            switch (strategy)
            {
                case SettingsStrategies.Local:
                    _container = ApplicationData.Current.LocalSettings;
                    break;
                case SettingsStrategies.Roam:
                    _container = ApplicationData.Current.RoamingSettings;
                    break;
                default:
                    throw new ArgumentException($"Unsupported Settings Strategy: {strategy}", nameof(strategy));
            }

            if (!string.IsNullOrWhiteSpace(folderName))
            {
                try
                {
                    _container = _container.CreateContainer(folderName, createFolderIfNotExists ? ApplicationDataCreateDisposition.Always : ApplicationDataCreateDisposition.Existing);
                }
                catch (Exception)
                {
                    throw new KeyNotFoundException($"No folder exists named '{folderName}'");
                }
            }
        }

        public ISettingsService Open(string folderName, bool createFolderIfNotExists = true) => new SettingsService(_strategy, folderName, createFolderIfNotExists);

        public IPropertySet Values => _container.Values;

        protected SettingsHelper Helper { get; private set; }

        public Func<string, ISettingConverter> GetConverter { get; set; } = p => new DefaultConverter();

        public virtual bool Exists(string key) => Values.ContainsKey(key);

        public virtual void Remove(string key) => Helper.Remove(key, Values, _container);

        public virtual void Clear(bool deep = true) => Helper.Clear(deep, Values, _container);

        public virtual void Write<T>(string key, T value) => Helper.Write(key, value, Values, GetConverter);

        public virtual T Read<T>(string key, T fallback = default(T)) => Helper.Read<T>(key, fallback, Values, GetConverter);

    }
}
