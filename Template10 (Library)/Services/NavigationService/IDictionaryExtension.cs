using System.Collections.Generic;

namespace Template10.Services.NavigationService
{
	public static class IDictionaryExtension
	{
		public static T GetOrDefault<T>(this IDictionary<string, object> state, string key, T defaultValue)
		{
			if (state.ContainsKey(key))
			{
				object o = state[key];

				if (o is T)
					return (T)o;
			}	

			return defaultValue;
		}
	}
}
