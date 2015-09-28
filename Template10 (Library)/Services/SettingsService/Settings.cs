using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
	public class SettingsService : PropertyStore
	{
		private static SettingsService _local;
		public static SettingsService Local
		{
			get
			{
				if (_local == null)
					_local = new SettingsService(ApplicationData.Current.LocalSettings.Values);
				return _local;
			}
		}

		private static SettingsService _roaming;
		public static SettingsService Roaming
		{
			get
			{
				if(_roaming == null)
					_roaming = new SettingsService(ApplicationData.Current.RoamingSettings.Values);

				return _roaming;
			}
		}

		private static readonly IPropertyMapping Mapping;

		static SettingsService()
		{
            // static constructor
			Mapping = new JsonMapping();
		}

		private SettingsService(IPropertySet values) : base(values, Mapping)
		{ /* private constructor */ }
	}
}
