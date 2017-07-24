using Cable.Tests.Models;
using NUnit.Framework;
using System.Linq;

namespace Cable.Tests
{
    [TestFixture]
    public class IEnumerableTests
    {
        [Test]
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

        [Test]
        public void ComplexTypeWithNullIsConvertedCorrectly()
        {
            var sample = new WithIEnumerable { Numbers = null };
            var serialized = Json.Serialize(sample);
            var deserialize = Json.Deserialize<WithIEnumerable>(serialized);
            Assert.IsTrue(deserialize.Numbers == null);
        }
    }
}
