using Cable.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Cable.Tests
{
    [TestClass]
    public class StringWithCharTests
    {
        [TestMethod]
        public void StringAndCharAreConvertedCorrectly()
        {
            var sample = new StringWithChar
            {
                String = "Text value",
                Char = 'a'
            };

            var serialized = JsonConvert.SerializeObject(sample, Formatting.None, new CableConverter());

            var deserialized = JsonConvert.DeserializeObject<StringWithChar>(serialized, new CableConverter());

            Assert.AreEqual(deserialized.Char.GetType(), typeof(char));
            Assert.AreEqual(deserialized.String.GetType(), typeof(string));
            Assert.AreEqual(deserialized.Char, sample.Char);
            Assert.AreEqual(deserialized.String, sample.String);
        }
    }
}
