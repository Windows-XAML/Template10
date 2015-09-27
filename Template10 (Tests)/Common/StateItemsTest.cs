using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template10.Common;

namespace Template10LibraryTests.Common
{
	[TestClass]
	public class StateItemsTest
	{
		StateItems Items;

		[TestInitialize]
		public void Initialize()
		{
			Items = new StateItems();
		}

		[TestCleanup]
		public void Cleanup()
		{
			Items = null;
		}

		[TestMethod]
		public void ContainsTest()
		{
			var item = Items.Add(typeof(StateItem), "Key", this);

			Assert.IsTrue(Items.Contains(item));
			Assert.IsTrue(Items.Contains(this));
			Assert.IsTrue(Items.Contains(typeof(StateItem), "Key"));
			Assert.IsTrue(Items.Contains(typeof(StateItem), "Key", this));
			Assert.IsTrue(Items.Contains<StateItem>(item));

			Items.Remove(this);
			Assert.IsFalse(Items.Contains(item));
			Assert.IsFalse(Items.Contains(this));
			Assert.IsFalse(Items.Contains(typeof(StateItem), "Key"));
			Assert.IsFalse(Items.Contains(typeof(StateItem), "Key", this));
			Assert.IsFalse(Items.Contains<StateItem>(item));
		}

		[TestMethod]
		public void RemoveTest()
		{
			var item = Items.Add(typeof(StateItem), "Key", this);

			Assert.IsTrue(Items.Contains(item));

			Items.Remove(typeof(StateItem));

			Assert.IsFalse(Items.Contains(item));

			Items.Add(item);

			Assert.IsTrue(Items.Contains(item));

			Items.Remove(this);

			Assert.IsFalse(Items.Contains(item));

			Items.Add(item);

			Assert.IsTrue(Items.Contains(item));

			Items.Remove(typeof(StateItem), "Key");

			Assert.IsFalse(Items.Contains(item));
		}

		[TestMethod]
		public void AddTest()
		{
			var item = Items.Add(typeof(StateItem), "Key", this);

			Assert.AreEqual("Key", item.Key);
			Assert.AreEqual(typeof(StateItem), item.Type);
			Assert.AreEqual(this, item.Value);

			Assert.ThrowsException<ArgumentException>(() => Items.Add(typeof(StateItem), "Key", this));

			// not sure about the behaviour
			Assert.ThrowsException<ArgumentException>(() => Items.Add(typeof(StateItem), "Key", "another object"));

			//FIXME default Add method allows duplicates
			Assert.ThrowsException<ArgumentException>(() => Items.Add(item));
		}

		[TestMethod]
		public void GetTest()
		{
			var item = Items.Add(typeof(StateItemsTest), "Key", this);
			var item2 = Items.Get<StateItemsTest>("Key");

			Assert.AreSame(this, item2);
		}

		[TestMethod]
		public void GetExceptionTest()
		{
			var item = Items.Add(typeof(StateItemsTest), "Key", this);

			//FIXME exception does not throw properly
			Assert.ThrowsException<KeyNotFoundException>(() => Items.Get<StateItemsTest>("Another"));
		}

		[TestMethod]
		public void TryGetTest()
		{
			var item = Items.Add(typeof(StateItemsTest), "Key", this);
			StateItemsTest item2;
			Items.TryGet<StateItemsTest>("Key",out item2);

			Assert.AreSame(this, item2);
		}

	}
}
