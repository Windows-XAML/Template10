using System;
using Windows.Foundation.Collections;

namespace Template10.Services.SettingsService
{
	public class PropertyStore
	{
		protected IPropertyMapping Converters { get; }

		protected IPropertySet Values { get; }

		public PropertyStore(IPropertySet values, IPropertyMapping converters)
		{
			if (values == null || converters == null)
				throw new ArgumentNullException();

			Values = values;
			Converters = converters;
		}

		public bool Exists(string key)
		{
			return Values.ContainsKey(key);
		}

		public void Remove(string key)
		{
			if (Values.ContainsKey(key))
				Values.Remove(key);
		}

		public void Write<T>(string key, T value)
		{
			var type = typeof(T);
			var converter = Converters.GetConverter(type);
			Values[key] = converter.ToStore(value, type);
		}

		public T Read<T>(string key, T fallback)
		{
			try
			{
				var type = typeof(T);
				var converter = Converters.GetConverter(type);
				return (T)converter.FromStore(Values[key].ToString(), type);

			} catch
			{
				return fallback;
			}
        }
	}
}
