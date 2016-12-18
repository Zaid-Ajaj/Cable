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
                assert.Equal(encoded["IsPrimitive"], true);
                assert.Equal(encoded["IsArray"], false);
                assert.Equal(encoded["IsNumeric"], false);
                assert.Equal(encoded["Type"], "DateTime");
                assert.Equal(encoded["Value"]["Year"], 2016);
                assert.Equal(encoded["Value"]["Month"], 12);
                assert.Equal(encoded["Value"]["Day"], 10);
                assert.Equal(encoded["Value"]["Hour"], 20);
                assert.Equal(encoded["Value"]["Minute"], 30);
                assert.Equal(encoded["Value"]["Second"], 0);
                assert.Equal(encoded["Value"]["Millisecond"], 235);
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