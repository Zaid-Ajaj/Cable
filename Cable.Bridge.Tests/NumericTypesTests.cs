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

                //@ console.log(encoded);

                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "UInt16");
                assert.Equal(encoded["Value"].As<string>(), "10");

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<ushort>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("UInt32 is converted correctly", assert =>
            {
                uint sample = 10;

                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "UInt32");
                assert.Equal(encoded["Value"].As<string>(), "10");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<uint>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("UInt64 is converted correctly", assert =>
            {
                ulong sample = 10;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "UInt64");
                assert.Equal(encoded["Value"].As<string>(), "10");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<ulong>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Int16 is converted correctly", assert =>
            {
                short sample = 2;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "Int16");
                assert.Equal(encoded["Value"].As<string>(), "2");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<short>(serialized);
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Int32 is coverted correctly", assert =>
            {
                var sample = 7;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "Int32");
                assert.Equal(encoded["Value"].As<string>(), "7");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<int>(serialized);
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Int64 is converted correctly", assert =>
            {
                var sample = 10L;

                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "Int64");
                assert.Equal(encoded["Value"].As<string>(), "10");

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<long>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Double is converted correctly", assert =>
            {
                var sample = 2.521;

                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "Double");
                assert.Equal(encoded["Value"].As<string>(), "2.521");

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<double>(serialized);
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Decimal is converted correctly", assert =>
            {
                var sample = 2.234m;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "Decimal");
                assert.Equal(encoded["Value"].As<string>(), "2.234");

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<decimal>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Byte is converted correctly", assert =>
            {
                byte sample = 200;
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"].As<bool>(), true);
                assert.Equal(encoded["IsArray"].As<bool>(), false);
                assert.Equal(encoded["IsNumeric"].As<bool>(), true);
                assert.Equal(encoded["Type"].As<string>(), "Byte");
                assert.Equal(encoded["Value"].As<string>(), "200");
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<byte>(serialized);
                assert.Equal(sample, deserialized);
            });
        }
    }
}