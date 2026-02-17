using NUnit.Framework;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace UniExtension
{
    public class CollectionExtensionTests
    {
        [SetUp]
        public void SetUp()
        {
            Random.InitState(12345);
        }

        [Test]
        public void GetRandomElement_List_ReturnsElementFromList()
        {
            var list = new List<int> { 10, 20, 30, 40 };

            var value = list.GetRandomElement();

            Assert.Contains(value, list);
        }

        [Test]
        public void GetRandomElement_HashSet_ReturnsElementFromCollection()
        {
            var set = new HashSet<string> { "A", "B", "C" };

            var value = set.GetRandomElement();

            Assert.IsTrue(set.Contains(value));
        }

        [Test]
        public void GetRandomElement_SingleElement_ReturnsThatElement()
        {
            var list = new List<int> { 42 };

            var value = list.GetRandomElement();

            Assert.AreEqual(42, value);
        }

        [Test]
        public void GetRandomElement_EmptyCollection_Throws()
        {
            var list = new List<int>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                list.GetRandomElement();
            });
        }

        [Test]
        public void TryGetRandomElement_List_ReturnsTrue()
        {
            var list = new List<int> { 1, 2, 3 };

            var success = list.TryGetRandomElement(out var value);

            Assert.IsTrue(success);
            Assert.Contains(value, list);
        }

        [Test]
        public void TryGetRandomElement_HashSet_ReturnsTrue()
        {
            var set = new HashSet<int> { 5, 6, 7 };

            var success = set.TryGetRandomElement(out var value);

            Assert.IsTrue(success);
            Assert.IsTrue(set.Contains(value));
        }

        [Test]
        public void TryGetRandomElement_SingleElement_ReturnsThatElement()
        {
            var list = new List<string> { "only" };

            var success = list.TryGetRandomElement(out var value);

            Assert.IsTrue(success);
            Assert.AreEqual("only", value);
        }

        [Test]
        public void TryGetRandomElement_EmptyCollection_ReturnsFalse()
        {
            var list = new List<int>();

            var success = list.TryGetRandomElement(out var value);

            Assert.IsFalse(success);
            Assert.AreEqual(default(int), value);
        }
    }
}
