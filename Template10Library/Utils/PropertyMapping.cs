using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Utils
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

	public class PropertyMapping : Dictionary<Type, IStoreConverter>, IPropertyMapping
	{
		protected IStoreConverter Fallback { get; }

		public PropertyMapping()
		{

			this[typeof(int)] = new IntConverter();
			this[typeof(string)] = new StringConverter();
			this[typeof(DateTime)] = new DateTimeConverter();
			this[typeof(DateTimeOffset)] = new DateTimeOffsetConverter();
			this[typeof(TimeSpan)] = new TimeSpanConverter();

			Fallback = new JsonConverter();
		}

		public IStoreConverter GetConverter(Type type)
		{
			return ContainsKey(type) ? this[type] : Fallback;
		}
	}

	public class IntConverter : IStoreConverter
	{
		public object FromStore(string value, Type type) => int.Parse(value);
		public string ToStore(object value, Type type) => value.ToString();
	}

	public class StringConverter : IStoreConverter
	{
		public object FromStore(string value, Type type) => value;
		public string ToStore(object value, Type type) => value.ToString();
	}

	public class DateTimeConverter : IStoreConverter
	{
		public object FromStore(string value, Type type) => new DateTime(long.Parse(value));
		public string ToStore(object value, Type type) => ((DateTime)value).Ticks.ToString();
	}

	public class DateTimeOffsetConverter : IStoreConverter
	{
		protected DateTimeConverter ProxyConverter = new DateTimeConverter();
		public object FromStore(string value, Type type) => new DateTimeOffset((DateTime)ProxyConverter.FromStore(value, type));
		public string ToStore(object value, Type type) => ((DateTimeOffset)value).Ticks.ToString();
	}

	public class TimeSpanConverter : IStoreConverter
	{
		public object FromStore(string value, Type type) => TimeSpan.FromTicks(long.Parse(value));
		public string ToStore(object value, Type type) => ((TimeSpan)value).Ticks.ToString();
	}

}
