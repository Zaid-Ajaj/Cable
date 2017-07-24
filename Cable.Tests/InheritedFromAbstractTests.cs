using NUnit.Framework;

using Cable.Tests.Models;
using Newtonsoft.Json;

namespace Cable.Tests
{
    [TestFixture]
    public class InheritedFromAbstractTests
    {
        [Test]
        public void InheritedFromAbstractIsConvertedCorrectly()
        {
            var sample = new InheritsFromAbstract
            {
                Id = 5,
                Value = "Value"
            };

            var serialized = JsonConvert.SerializeObject(sample, Formatting.None, new CableConverter());

            var deserialized = JsonConvert.DeserializeObject<InheritsFromAbstract>(serialized, new CableConverter());

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(deserialized.Id, sample.Id);
            Assert.AreEqual(deserialized.Value, sample.Value);
        }
    }
}
