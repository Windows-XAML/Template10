using System;

namespace Template10.Services.SettingsService
{
    public interface IPropertyMapping
    {
        IStoreConverter GetConverter(Type type);
    }
}