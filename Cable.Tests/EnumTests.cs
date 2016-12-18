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
    public class EnumTests
    {
        [TestMethod]
        public void EnumIsConvertedCorrectly()
        {
            var sample = new WithEnum { Choice = Choices.One };

            var serialized = JsonConvert.SerializeObject(sample, Formatting.Indented, new CableConverter());

            var deserialized = JsonConvert.DeserializeObject<WithEnum>(serialized, new CableConverter());

            Assert.AreEqual(sample.Choice, deserialized.Choice);
        }
    }
}
