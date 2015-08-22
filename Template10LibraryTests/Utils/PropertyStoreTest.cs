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
	public class PropertyStoreTest
	{

		[TestMethod]
		public void ConstructorTest()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new PropertyStore(null, new PropertyMapping()));
		}

		[TestMethod]
		public void ReadWriteTest()
		{
			var values = new PropertySet();
			var store = new PropertyStore(values, new PropertyMapping());

			store.Write("stringKey", "stringValue");
			store.Write("intKey", 42);

			var date = DateTime.Now;
			store.Write("dateKey", date);

			var dateOffset = DateTimeOffset.Now;
			store.Write("dateOffsetKey", dateOffset);

			double doub = 2.34566;
			store.Write("doubleKey", doub);


			Assert.AreEqual("stringValue", store.Read("stringKey", "none"));
			Assert.AreEqual(42, store.Read("intKey", -1));
			Assert.AreEqual(date, store.Read("dateKey", new DateTime()));
			Assert.AreEqual(dateOffset, store.Read("dateOffsetKey", dateOffset));
			Assert.AreEqual(doub, store.Read("doubleKey", 0.0));
		}
	}
}
