using System;

namespace Template10.Services.SettingsService
{
    public interface ISettingConverter
    {
        string ToStore<T>(T value);
        T FromStore<T>(string value);
    }
}