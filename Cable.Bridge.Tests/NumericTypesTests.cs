using Bridge;
using Bridge.QUnit;
using System;

namespace Cable.Bridge.Tests
{
    public static class NumericTypesTests
    {
        public static void Run()
        {
            QUnit.Module("Numeric types Tests");

            QUnit.Test("UInt16 is converted correctly", assert =>
            {
                ushort sample = 10;
                assert.Equal(sample.GetType().Name, typeof(ushort).Name);
                assert.Equal(sample.GetType().Name, "UInt16");
                var encoded = Converters.EncodeObject(sample);

                assert.Equal(encoded["Type"].As<string>(), "UInt16");
                assert.Equal(encoded["Value"].As<string>(), "10");

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(ushort)).As<ushort>();
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("UInt32 is converted correctly", assert =>
            {
                uint sample = 10;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["Type"].As<string>(), "UInt32");
                assert.Equal(encoded["Value"].As<string>(), "10");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(uint)).As<uint>();
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("UInt64 is converted correctly", assert =>
            {
                ulong sample = 10;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["Type"].As<string>(), "UInt64");
                assert.Equal(encoded["Value"].As<string>(), "10");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(ulong)).As<ulong>();
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Int16 is converted correctly", assert =>
            {
                short sample = 2;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["Type"].As<string>(), "Int16");
                assert.Equal(encoded["Value"].As<string>(), "2");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(short)).As<short>();
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Int32 is coverted correctly", assert =>
            {
                var sample = 7;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["Type"].As<string>(), "Int32");
                assert.Equal(encoded["Value"].As<string>(), "7");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(int)).As<int>();
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Int64 is converted correctly", assert =>
            {
                var sample = 10L;

                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["Type"].As<string>(), "Int64");
                assert.Equal(encoded["Value"].As<string>(), "10");

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(long)).As<long>();
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Double is converted correctly", assert =>
            {
                var sample = 2.521;

                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["Type"].As<string>(), "Double");
                assert.Equal(encoded["Value"].As<string>(), "2.521");

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(double)).As<double>();
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Decimal is converted correctly", assert =>
            {
                var sample = 2.234m;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["Type"].As<string>(), "Decimal");
                assert.Equal(encoded["Value"].As<string>(), "2.234");

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(decimal)).As<decimal>();
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Byte is converted correctly", assert =>
            {
                byte sample = 200;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["Type"].As<string>(), "Byte");
                assert.Equal(encoded["Value"].As<string>(), "200");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize(serialized, typeof(byte)).As<byte>();
                assert.Equal(sample, deserialized);
            });
        }
    }
}