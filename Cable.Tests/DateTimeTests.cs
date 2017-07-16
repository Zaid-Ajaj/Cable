using System;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cable.Tests
{

    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void DateTimeIsConvertedCorrectly()
        {
            var now = DateTime.Now;

            string serialized = Json.Serialize(now);

            var deserialized = Json.Deserialize<DateTime>(serialized);

            Assert.AreEqual(now.Year, deserialized.Year);
            Assert.AreEqual(now.Month, deserialized.Month);
            Assert.AreEqual(now.Day, deserialized.Day);
            Assert.AreEqual(now.Hour, deserialized.Hour);
            Assert.AreEqual(now.Minute, deserialized.Minute);
            Assert.AreEqual(now.Second, deserialized.Second);
            Assert.AreEqual(now.Millisecond, deserialized.Millisecond);
        }


        [TestMethod]
        public void DateTimeOfKindUTCConvertedCorrectly()
        {
            var now = DateTime.Now.ToUniversalTime();

            string serialized = Json.Serialize(now);
            var deserialized = Json.Deserialize<DateTime>(serialized);

            Assert.AreEqual(now.Year, deserialized.Year);
            Assert.AreEqual(now.Month, deserialized.Month);
            Assert.AreEqual(now.Day, deserialized.Day);
            Assert.AreEqual(now.Hour, deserialized.Hour);
            Assert.AreEqual(now.Minute, deserialized.Minute);
            Assert.AreEqual(now.Second, deserialized.Second);
            Assert.AreEqual(now.Millisecond, deserialized.Millisecond);
        }
    }
}
