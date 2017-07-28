using Bridge;
using Bridge.QUnit;
using Cable.Bridge;
using System;

namespace Cable.Bridge.Tests
{
    public static class DateTimeTests
    {
        static void DateTimeIsEncodedCorrectly()
        {
            var time = new DateTime(2016, 12, 10, 20, 30, 0, 235);

            var encoded = Converters.EncodeDateTime(time);

            
            QUnit.Test("Encoding DateTime works", assert =>
            {
                assert.Equal(encoded["Type"], "DateTime");
                assert.Equal(encoded["Value"], time.ToString("dd/MM/yyyy HH:mm:ss.fff"));
            });

            QUnit.Test("Encoding boxed DateTime works", assert =>
            {
                object boxed = time;

                var boxedEncoded = Converters.EncodeDateTime(boxed.As<DateTime>());

                assert.DeepEqual(encoded, boxedEncoded);
            });

            QUnit.Test("Encode -> Decode DateTime works", assert =>
            {
                var now = DateTime.Now;

                var serialized = Json.Serialize(now);

                var deserialized = Json.Deserialize<DateTime>(serialized);

                assert.Equal(now.Year, deserialized.Year);
                assert.Equal(now.Month, deserialized.Month);
                assert.Equal(now.Day, deserialized.Day);
                assert.Equal(now.Hour, deserialized.Hour);
                assert.Equal(now.Minute, deserialized.Minute);
                assert.Equal(now.Second, deserialized.Second);
                assert.Equal(now.Millisecond, deserialized.Millisecond);

            });
            
        }

        public static void Run()
        {
            QUnit.Module("DateTime Tests");
            DateTimeIsEncodedCorrectly();
        }
    }
}