using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.Foundation.Collections;

namespace Template10LibraryTests.Utils
{
	[TestClass]
	public class PropertyStoreTest : JsonStoreTest
	{
		//TODO nullable types

		public override void SetUp()
		{
			Values = new PropertySet();
			Store = new PropertyStore(Values, new PropertyMapping());
		}

		[TestMethod]
		public override void ConstructorTest()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new PropertyStore(null, new PropertyMapping()));
		}
		
		[TestMethod]
		public override void ReadWriteBooleanTest()
		{
			base.ReadWriteBooleanTest();
		}

		[TestMethod]
		public override void ReadWriteIntTest()
		{
			base.ReadWriteIntTest();
		}

		[TestMethod]
		public override void ReadWriteStringTest()
		{
			base.ReadWriteStringTest();
		}

		[TestMethod]
		public override void ReadWriteFloatTest()
		{
			base.ReadWriteFloatTest();
		}

		[TestMethod]
		public override void ReadWriteEnumTest()
		{
			base.ReadWriteEnumTest();
		}

		[TestMethod]
		public override void ReadWriteDateTest()
		{
			base.ReadWriteDateTest();
		}
	}
}
