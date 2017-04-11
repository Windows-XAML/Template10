using System;

namespace Template10.Services.SettingsService
{
    public interface ISettingMapping
    {
        ISettingConverter GetConverter(Type type);
    }
}