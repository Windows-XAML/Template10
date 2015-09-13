using System;
using System.Linq;
using System.Reflection;

namespace Template10.Utils
{
	public static class TypeUtil
	{
		readonly static Type[] Primitives;

		readonly static Type[] NullablePrimitives;

		readonly static Type[] AllPrimitives;

		static TypeUtil()
		{
			Primitives = new Type[]
			{
				typeof (Enum),
				typeof (String),
				typeof (Char),
				typeof (Guid),

				typeof (Boolean),
				typeof (Byte),
				typeof (Int16),
				typeof (Int32),
				typeof (Int64),
				typeof (Single),
				typeof (Double),
				typeof (Decimal),

				typeof (SByte),
				typeof (UInt16),
				typeof (UInt32),
				typeof (UInt64),

				typeof (DateTime),
				typeof (DateTimeOffset),
				typeof (TimeSpan),
			};

			NullablePrimitives = (from t in Primitives
								  where t.GetTypeInfo().IsValueType
								  select typeof(Nullable<>).MakeGenericType(t)).ToArray();

			AllPrimitives = Primitives.Concat(NullablePrimitives).ToArray();
		}

		public static bool IsPrimitive(Type type)
		{
			if (AllPrimitives.Any(x => x.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())))
				return true;

			var nullable = Nullable.GetUnderlyingType(type);

			return nullable != null && nullable.GetTypeInfo().IsEnum;
		}
	}
}
