using Bridge;
using Bridge.QUnit;


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
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<ushort>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("UInt32 is converted correctly", assert =>
            {
                uint sample = 10;
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<uint>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("UInt64 is converted correctly", assert =>
            {
                ulong sample = 10;
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<ulong>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Int16 is converted correctly", assert =>
            {
                short sample = 2;
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<short>(serialized);
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Int32 is coverted correctly", assert =>
            {
                var sample = 7;
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<int>(serialized);
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Int64 is converted correctly", assert =>
            {
                var sample = 10L;
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<long>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Double is converted correctly", assert =>
            {
                var sample = 2.0;
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<double>(serialized);
                assert.Equal(sample, deserialized);
            });

            QUnit.Test("Decimal is converted correctly", assert =>
            {
                var sample = 2.0m;
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<decimal>(serialized);
                assert.Equal(sample == deserialized, true);
            });

            QUnit.Test("Byte is converted correctly", assert =>
            {
                byte sample = 200;
                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<byte>(serialized);
                assert.Equal(sample, deserialized);
            });
        }
    }
}