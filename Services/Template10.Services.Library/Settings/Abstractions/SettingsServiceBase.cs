using Template10.Extensions;
using Template10.Services.Serialization;
using Windows.Storage;

namespace Template10.Services.Settings
{
    public abstract class SettingsServiceBase
    {
        protected ISettingsHelper Helper { get; private set; }

        protected bool EnableCompression
        {
            get => Helper.EnableCompression;
            set => Helper.EnableCompression = value;
        }

        public SettingsServiceBase(ISettingsAdapter adapter, ISerializationService serial)
        {
            Helper = new SettingsHelper(adapter, serial);
        }

        public SettingsServiceBase(ISettingsHelper helper)
        {
            Helper = helper;
        }
    }
}
