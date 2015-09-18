using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Template10.Utils;

namespace Template10LibraryTests.Utils
{
	[TestClass]
	public class TypeUtilTest
	{
		[TestMethod]
		public void TestPrimitiveTypes()
		{
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Enum)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(String)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Char)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Guid)));

			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Boolean)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Byte)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Int16)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Int32)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Int64)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Single)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Double)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(Decimal)));

			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(SByte)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(UInt16)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(UInt32)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(UInt64)));

			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(DateTime)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(DateTimeOffset)));
			Assert.IsTrue(TypeUtil.IsPrimitive(typeof(TimeSpan)));

			Assert.IsFalse(TypeUtil.IsPrimitive(typeof(TypeUtil)));
		}
	}
}
