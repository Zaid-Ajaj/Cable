using Bridge.QUnit;
using Cable;
using System;

namespace Cable.Bridge.Tests
{
    public static class TimeSpanTests
    {
        public static void Run()
        {
            QUnit.Module("TimeSpan Tests");

            QUnit.Test("TimeSpan is converted correctly", assert =>
            {
                var timeSpan = TimeSpan.FromHours(5);

                var serialized = Json.Serialize(timeSpan);

                var deserialized = Json.Deserialize<TimeSpan>(serialized);

                assert.Equal(true, timeSpan.Ticks == deserialized.Ticks);

            });
        }
    }
}