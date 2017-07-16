using Cable.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Tests
{
    [TestClass]
    public class IEnumerableTests
    {
        [TestMethod]
        public void IEnumerableIsConvertedCorrectly()
        {
            var sample = new WithIEnumerable { Numbers = Enumerable.Range(1, 10) };

            var serialized = Json.Serialize(sample);
            var deserialized = Json.Deserialize<WithIEnumerable>(serialized);

            var isValid =
                    deserialized.Numbers.Count() == sample.Numbers.Count()
                 && deserialized.Numbers.All(number => sample.Numbers.Contains(number))
                 && sample.Numbers.All(number => deserialized.Numbers.Contains(number));

            Assert.AreEqual(isValid, true);
        }

        [TestMethod]
        public void ComplexTypeWithNullIsConvertedCorrectly()
        {
            var sample = new WithIEnumerable { Numbers = null };
            var serialized = Json.Serialize(sample);
            var deserialize = Json.Deserialize<WithIEnumerable>(serialized);
            Assert.IsTrue(deserialize.Numbers == null);
        }
    }
}
