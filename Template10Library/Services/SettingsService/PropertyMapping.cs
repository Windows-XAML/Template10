using Newtonsoft.Json;
using System;

namespace Template10.Services.SettingsService
{
	public interface IStoreConverter
	{
		string ToStore(object value, Type type);
		object FromStore(string value, Type type);
	}

	public interface IPropertyMapping
	{
		IStoreConverter GetConverter(Type type);
	}

	public class JsonConverter : IStoreConverter
	{
		public object FromStore(string value, Type type) => JsonConvert.DeserializeObject(value, type);
		public string ToStore(object value, Type type) => JsonConvert.SerializeObject(value);
	}

	public class JsonMapping : IPropertyMapping
	{
		protected IStoreConverter jsonConverter = new JsonConverter();

		public IStoreConverter GetConverter(Type type)
		{
			return jsonConverter;
		}
	}
}
