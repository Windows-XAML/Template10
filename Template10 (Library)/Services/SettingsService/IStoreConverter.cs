using System;

namespace Template10.Services.SettingsService
{
    public interface IStoreConverter
    {
        string ToStore(object value, Type type);
        object FromStore(string value, Type type);
    }
}