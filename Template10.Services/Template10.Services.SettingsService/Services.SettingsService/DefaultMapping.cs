using System;

namespace Template10.Services.SettingsService
{
    public class DefaultMapping : ISettingMapping 
    {
        protected ISettingConverter jsonConverter = new DefaultConverter();
        public ISettingConverter GetConverter(Type type) => this.jsonConverter;
    }
}