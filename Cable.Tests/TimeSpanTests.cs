using NUnit.Framework;
using System;

namespace Cable.Tests
{
    [TestFixture]
    public class TimeSpanTests
    {
        [Test]
        public void TimeSpanIsConvertedCorrectly()
        {
            var span = TimeSpan.FromDays(1000);

            var serialized = Json.Serialize(span);

            var deserialized = Json.Deserialize<TimeSpan>(serialized);

            Assert.AreEqual(span.Ticks, deserialized.Ticks);
        }
    }
}
