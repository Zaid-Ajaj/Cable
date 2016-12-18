using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Tests
{
    [TestClass]
    public class TimeSpanTests
    {
        [TestMethod]
        public void TimeSpanIsConvertedCorrectly()
        {
            var span = TimeSpan.FromDays(1000);

            var serialized = Json.Serialize(span);

            var deserialized = Json.Deserialize<TimeSpan>(serialized);

            Assert.AreEqual(span.Ticks, deserialized.Ticks);
        }
    }
}
