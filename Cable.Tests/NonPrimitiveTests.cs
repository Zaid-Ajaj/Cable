using Cable.Tests.Models;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Cable.Tests
{
    [TestFixture]
    public class NonPrimitiveTests
    {
        static NonPrimitive Sample() => new NonPrimitive { Id = 5 };

        [Test]
        public void TypeInformationIsAddedToNonPrimitiveType()
        {
            var sample = Sample();
            var serialized = Json.Serialize(sample);
            var json = JObject.Parse(serialized);         
            Assert.AreEqual(json["Value"]["Id"]["Type"].Value<string>(), "Int32");
            Assert.AreEqual(json["Value"]["Id"]["Value"].Value<int>(), 5);
        }


        [Test]
        public void NonPrimitiveIsDeserializedToDynamic()
        {
            var sample = Sample();

            var serialized = Json.Serialize(sample);

            dynamic deserialized = Json.Deserialize<dynamic>(serialized);

            Assert.AreEqual(sample.Id, deserialized.Id);
        }
    }
}
