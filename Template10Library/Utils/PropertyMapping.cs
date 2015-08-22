using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Utils
{
	public class PropertyMapping : Dictionary<Type, IStoreConverter>
	{
		public PropertyMapping()
		{
			this[typeof(int)] = new IntConverter();
			this[typeof(string)] = new StringConverter();
			this[typeof(DateTime)] = new DateTimeConverter();
			this[typeof(DateTimeOffset)] = new DateTimeOffsetConverter();
			this[typeof(TimeSpan)] = new TimeSpanConverter();
		}
	}

	public interface IStoreConverter
	{
		string ToStore(object value);

		object FromStore(string value);
	}

	public class IntConverter : IStoreConverter
	{
		public object FromStore(string value) => int.Parse(value);
		public string ToStore(object value) => value.ToString();
	}

	public class StringConverter : IStoreConverter
	{
		public object FromStore(string value) => value;
		public string ToStore(object value) => value.ToString();
	}

	public class DateTimeConverter : IStoreConverter
	{
		public object FromStore(string value) => new DateTime(long.Parse(value));
		public string ToStore(object value) => ((DateTime)value).Ticks.ToString();
	}

	public class DateTimeOffsetConverter : IStoreConverter
	{
		protected DateTimeConverter ProxyConverter = new DateTimeConverter();
		public object FromStore(string value) => new DateTimeOffset((DateTime)ProxyConverter.FromStore(value));
		public string ToStore(object value) => ((DateTimeOffset)value).Ticks.ToString();
	}

	public class TimeSpanConverter : IStoreConverter
	{
		public object FromStore(string value) => TimeSpan.FromTicks(long.Parse(value));
		public string ToStore(object value) => ((TimeSpan)value).Ticks.ToString();
	}
}
