using System;
using System.Linq;
using System.Reflection;

namespace Template10.Extensions
{
    public static class TypeExtensions
	{
		readonly static Type[] _primitives;

		readonly static Type[] _nullablePrimitives;

		readonly static Type[] _allPrimitives;

		static TypeExtensions()
		{
			_primitives = new Type[]
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

			_nullablePrimitives = (from t in _primitives
								  where t.GetTypeInfo().IsValueType
								  select typeof(Nullable<>).MakeGenericType(t)).ToArray();

			_allPrimitives = _primitives.Concat(_nullablePrimitives).ToArray();
		}

		public static bool IsPrimitive(this Type type)
		{
			if (_allPrimitives.Any(x => x.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())))
            {
                return true;
            }

            var nullable = Nullable.GetUnderlyingType(type);

			return nullable != null && nullable.GetTypeInfo().IsEnum;
		}
	}
}
