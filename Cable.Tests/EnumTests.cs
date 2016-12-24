using Cable.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cable.Tests
{
    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void EnumIsConvertedCorrectly()
        {
            var sample = new WithEnum { Choice = Choices.One };

            var serialized = Json.Serialize(sample);

            var deserialized = Json.Deserialize<WithEnum>(serialized);

            Assert.AreEqual(sample.Choice, deserialized.Choice);
        }
    }
}
