using Template10.Utils;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
	public class Settings : PropertyStore
	{
		public static readonly Settings Local;
        public static readonly Settings Roaming;

		private static readonly IPropertyMapping Mapping;

		static Settings()
		{
			Mapping = new JsonMapping();
			Local = new Settings(ApplicationData.Current.LocalSettings.Values);
			Roaming = new Settings(ApplicationData.Current.RoamingSettings.Values);
		}

		private Settings(IPropertySet values) : base(values, Mapping)
		{ }
	}
}
