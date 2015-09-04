using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Template10.Common;

namespace Template10LibraryTests.Common
{
	[TestClass]
	public class StateItemTest
	{
		[TestMethod]
		public void DefaultValuesTest()
		{
			var item = new StateItem();

			Assert.AreEqual(null, item.Type);
			Assert.AreEqual(null, item.Key);
			Assert.AreEqual(null, item.Value);
		}

		[TestMethod]
		public void GetterSetterTest()
		{
			var item = new StateItem();

			item.Key = "Key";
			item.Type = typeof(StateItem);
			item.Value = this;

			Assert.AreEqual("Key", item.Key);
			Assert.AreEqual(typeof(StateItem), item.Type);
			Assert.AreEqual(this, item.Value);
		}
	}
}
