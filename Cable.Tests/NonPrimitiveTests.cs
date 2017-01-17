using Cable.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Tests
{
    [TestClass]
    public class NonPrimitiveTests
    {
        static NonPrimitive Sample() => new NonPrimitive { Id = 5 };

        [TestMethod]
        public void TypeInformationIsAddedToNonPrimitiveType()
        {
            var sample = Sample();

            var serialized = Json.Serialize(sample);

            var json = JObject.Parse(serialized);

            Assert.AreEqual(json["IsPrimitive"].Value<bool>(), false);
            Assert.AreEqual(json["IsNumeric"].Value<bool>(), false);
            Assert.AreEqual(json["IsArray"].Value<bool>(), false);            
            Assert.AreEqual(json["Value"]["Id"]["IsPrimitive"].Value<bool>(), true);
            Assert.AreEqual(json["Value"]["Id"]["IsNumeric"].Value<bool>(), true);
            Assert.AreEqual(json["Value"]["Id"]["IsArray"].Value<bool>(), false);
            Assert.AreEqual(json["Value"]["Id"]["Type"].Value<string>(), "Int32");
            Assert.AreEqual(json["Value"]["Id"]["Value"].Value<int>(), 5);
        }


        [TestMethod]
        public void NonPrimitiveIsDeserializedToDynamic()
        {
            var sample = Sample();

            var serialized = Json.Serialize(sample);

            dynamic deserialized = Json.Deserialize<dynamic>(serialized);

            Assert.AreEqual(sample.Id, deserialized.Id);
        }
    }
}
