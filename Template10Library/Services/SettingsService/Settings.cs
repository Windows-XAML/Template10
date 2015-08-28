using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
	// lazy initialization: https://github.com/Windows-XAML/Template10/pull/50
	public class Settings : PropertyStore
	{
		private static Settings _local;
		public static Settings Local
		{
			get
			{
				if (_local == null)
					_local = new Settings(ApplicationData.Current.LocalSettings.Values);

				return _local;
			}
		}

		private static Settings _roaming;
		public static Settings Roaming
		{
			get
			{
				if(_roaming == null)
					_roaming = new Settings(ApplicationData.Current.RoamingSettings.Values);

				return _roaming;
			}
		}

		private static readonly IPropertyMapping Mapping;

		static Settings()
		{
			Mapping = new JsonMapping();
		}

		private Settings(IPropertySet values) : base(values, Mapping)
		{ }
	}
}
