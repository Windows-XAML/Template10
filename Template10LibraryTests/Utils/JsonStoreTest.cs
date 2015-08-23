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
	public class JsonStoreTest
	{
		public IPropertySet Values;
		public PropertyStore Store;

		public virtual void SetUp()
		{
			Values = new PropertySet();
			Store = new PropertyStore(Values, new JsonMapping());
		}

		public virtual void TearDown()
		{
			Values = null;
			Store = null;
		}

		[TestMethod]
		public virtual void ConstructorTest()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new PropertyStore(null, new JsonMapping()));
		}

		[TestMethod]
		public virtual void ReadWriteBooleanTest()
		{
			SetUp();

			var key = "booleanKey";
			Store.Write(key, true);
			Assert.IsTrue(Store.Exists(key));
			Assert.AreEqual(true, Store.Read(key, false));

			Store.Write(key, false);
			Assert.AreEqual(false, Store.Read(key, true));

			Store.Remove(key);
			Assert.IsFalse(Store.Exists(key));
			Assert.AreEqual(true, Store.Read(key, true));

			TearDown();
		}

		[TestMethod]
		public virtual void ReadWriteIntTest()
		{
			SetUp();

			var key = "intKey";
			var value = 42;
			var empty = -1;

			Store.Write(key, value);
			Assert.IsTrue(Store.Exists(key));
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Write(key, value = 24);
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Remove(key);
			Assert.IsFalse(Store.Exists(key));
			Assert.AreEqual(empty, Store.Read(key, empty));

			TearDown();
		}

		[TestMethod]
		public virtual void ReadWriteStringTest()
		{
			SetUp();

			var key = "stringKey";
			var value = "stringValue";
			var empty = string.Empty;

			Store.Write(key, value);
			Assert.IsTrue(Store.Exists(key));
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Write(key, value = "newValue");
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Remove(key);
			Assert.IsFalse(Store.Exists(key));
			Assert.AreEqual(empty, Store.Read(key, empty));

			TearDown();
		}

		[TestMethod]
		public virtual void ReadWriteFloatTest()
		{
			SetUp();

			var key = "floatKey";
			var value = 1.23456789;
			var empty = 0.0;

			Store.Write(key, value);
			Assert.IsTrue(Store.Exists(key));
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Write(key, value = 0.1);
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Remove(key);
			Assert.IsFalse(Store.Exists(key));
			Assert.AreEqual(empty, Store.Read(key, empty));

			TearDown();
		}

		[TestMethod]
		public virtual void ReadWriteEnumTest()
		{
			SetUp();

			var key = "enumKey";
			var value = TestEnum.One;
			var empty = TestEnum.Empty;

			Store.Write(key, value);
			Assert.IsTrue(Store.Exists(key));
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Write(key, value = TestEnum.Three);
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Remove(key);
			Assert.IsFalse(Store.Exists(key));
			Assert.AreEqual(empty, Store.Read(key, empty));

			TearDown();
		}

		[TestMethod]
		public virtual void ReadWriteDateTest()
		{
			SetUp();

			var key = "dateKey";
			DateTime? value = DateTime.Now;
			DateTime? empty = null;

			Store.Write(key, value);
			Assert.IsTrue(Store.Exists(key));
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Write(key, value = value.Value.AddMonths(12));
			Assert.AreEqual(value, Store.Read(key, empty));

			Store.Write(key, empty);
			Assert.AreEqual(empty, Store.Read(key, empty));

			Store.Remove(key);
			Assert.IsFalse(Store.Exists(key));
			Assert.AreEqual(empty, Store.Read(key, empty));

			TearDown();
		}

	}

	public enum TestEnum
	{
		Empty, One, Two, Three
	}
}
