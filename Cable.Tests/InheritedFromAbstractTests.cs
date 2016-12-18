using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cable.Tests.Models;
using Newtonsoft.Json;

namespace Cable.Tests
{
    [TestClass]
    public class InheritedFromAbstractTests
    {
        [TestMethod]
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
