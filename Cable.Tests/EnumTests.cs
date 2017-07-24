using Cable.Tests.Models;
using NUnit.Framework;

namespace Cable.Tests
{
    [TestFixture]
    public class EnumTests
    {
        [Test]
        public void EnumIsConvertedCorrectly()
        {
            var sample = new WithEnum { Choice = Choices.One };

            var serialized = Json.Serialize(sample);

            var deserialized = Json.Deserialize<WithEnum>(serialized);

            Assert.AreEqual(sample.Choice, deserialized.Choice);
        }
    }
}
