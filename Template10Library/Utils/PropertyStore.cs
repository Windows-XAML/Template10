using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;

namespace Template10.Utils
{
	public class PropertyStore
	{
		protected Dictionary<Type, IStoreConverter> Converters { get; }

		protected IPropertySet Values { get; }

		public PropertyStore(IPropertySet values, Dictionary<Type, IStoreConverter> converters)
		{
			if (values == null)
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

			if(Converters.ContainsKey(type))
			{
				Values[key] = Converters[type].ToStore(value);
			}
			else
			{
				var json = Serialize(value);
				Values[key] = json; 
			}
		}

		public T Read<T>(string key, T fallback)
		{
			try
			{
				var type = typeof(T);

				if (Converters.ContainsKey(type))
				{
					return (T)Converters[type].FromStore(Values[key] as string);
				}
				else
				{
					var json = Values[key].ToString();
					return Deserialize<T>(json);
				}

			} catch
			{
				return fallback;
			}
        }

		protected static string Serialize<T>(T item)
		{
			return JsonConvert.SerializeObject(item);
		}

		protected static T Deserialize<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}

}
