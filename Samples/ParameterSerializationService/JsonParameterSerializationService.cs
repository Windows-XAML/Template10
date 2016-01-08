using System;

namespace Neusta.Commons.Template10.Utils
{
	using System.Text;
	using global::Template10.Services.NavigationService;
	using Newtonsoft.Json;

	public class JsonParameterSerializationService : ParameterSerializationService
	{
		private volatile Tuple<object, string> lastObject = new Tuple<object, string>(null, null);

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonParameterSerializationService"/> class.
		/// </summary>
		private JsonParameterSerializationService()
		{
		}

		/// <summary>
		/// Initializes this service.
		/// </summary>
		public static void Initialize()
		{
			ParameterSerializationService.Instance = new JsonParameterSerializationService();
		}

		#region Overrides of ParameterSerializationService

		/// <summary>
		/// Serializes the page parameter.
		/// </summary>
		public override object SerializeParameter(object parameter)
		{
			if (parameter.IsNull())
			{
				return null;
			}
			var parameterString = parameter as string;
			if (parameterString.IsNotNull())
			{
				return parameterString;
			}

			// Check for last object
			var last = this.lastObject;
			if (last.Item1 == parameter)
			{
				return last.Item2;
			}

			// Serialize object to json
			var sb = new StringBuilder();
			sb.Append((char)9);
			sb.Append(parameter.GetType().AssemblyQualifiedName);
			sb.Append((char)9);
			string data = JsonConvert.SerializeObject(parameter);
			sb.Append(data);
			parameterString = sb.ToString();

			// Save last object
			this.lastObject = new Tuple<object, string>(parameter, parameterString);

			return parameterString;
		}

		/// <summary>
		/// Deserializes the page parameter.
		/// </summary>
		public override object DeserializeParameter(object parameter)
		{
			var parameterString = parameter as string;
			if (string.IsNullOrEmpty(parameterString) || (parameterString[0] != (char)9))
			{
				return parameter;
			}

			// Check for last object
			var last = this.lastObject;
			if (last.Item2 == parameterString)
			{
				return last.Item1;
			}

			// Try to deserialize object from json
			try
			{
				int idx = parameterString.IndexOf((char)9, 2);
				string typeName = parameterString.Substring(1, idx - 1);
				Type type = Type.GetType(typeName);
				string data = parameterString.Substring(idx + 1);
				parameter = JsonConvert.DeserializeObject(data, type);
			}
			catch
			{
				parameter = null;
			}

			// Save last object
			this.lastObject = new Tuple<object, string>(parameter, parameterString);

			return parameter;
		}

		#endregion
	}
}