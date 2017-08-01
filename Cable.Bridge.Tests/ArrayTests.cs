using Bridge;
using Bridge.QUnit;
using System;

namespace Cable.Bridge.Tests
{
    public static class ArrayTests
    {
        static void ArrayIsEncodedCorrectly()
        {
            var arr = new int[] { 1, 2, 3 };
            var encoded = Converters.EncodeObject(arr);

            QUnit.Test("Array of ints is encoded correctly", assert =>
            {
                assert.Equal(encoded["Type"], "Array");
                assert.Equal(encoded["Value"]["length"], arr.Length);

                var values = encoded["Value"].As<object[]>();

                for (int i = 0; i < values.Length; i++)
                {
                    var intJson = values[i];
                    assert.Equal(intJson["Type"], "Int32");
                    assert.Equal(intJson["Value"], i + 1);
                }
            });
        }



        public static void Run()
        {
            QUnit.Module("Array Tests");
            ArrayIsEncodedCorrectly();

            QUnit.Test("Serialization and deserialization of int array works", assert =>
            {
                var ints = new int[] { 1, 2, 3 };
                var serialized = Json.Serialize(ints);
                var deserialized = Json.Deserialize<int[]>(serialized);
                assert.DeepEqual(deserialized, ints);
            });


            QUnit.Test("Serialization and deserialization of string array works", assert =>
            {
                var strings = new string[] { "one", "two" };
                var serialized = Json.Serialize(strings);
                var deserialized = Json.Deserialize<string[]>(serialized);
                assert.DeepEqual(deserialized, strings);
            });

            QUnit.Test("Serialization and deserialization of long array works", assert =>
            {
                var longs = new long[] { 10L, 20L, 30L };
                var serialized = Json.Serialize(longs);
                var deserialized = Json.Deserialize<long[]>(serialized);

                assert.Equal(longs[0] == deserialized[0], true);
                assert.Equal(longs[1] == deserialized[1], true);
                assert.Equal(longs[2] == deserialized[2], true);
            });

            QUnit.Test("Serialization and deserialization of double[] works", assert =>
            {
                var doubles = new double[] { 2.5, 2.56, 2.567 };
                var serialized = Json.Serialize(doubles);
                var deserialized = Json.Deserialize<double[]>(serialized);

                assert.Equal(doubles[0] == deserialized[0], true);
                assert.Equal(doubles[1] == deserialized[1], true);
                assert.Equal(doubles[2] == deserialized[2], true);

            });

            QUnit.Test("Serialization and deserialization of decimal[] works", assert =>
            {
                var decimals = new decimal[] { 2.5m, 2.56m, 2.567m };
                var serialized = Json.Serialize(decimals);
                var deserialized = Json.Deserialize<decimal[]>(serialized);

                assert.Equal(decimals[0] == deserialized[0], true);
                assert.Equal(decimals[1] == deserialized[1], true);
                assert.Equal(decimals[2] == deserialized[2], true);

            });

            QUnit.Test("Serialization and deserialization of DateTime array works", assert =>
            {
                var dates = new DateTime[] { DateTime.Now, new DateTime(2016, 12, 10, 20, 30, 0, 15) };
                var serialized = Json.Serialize(dates);
                var deserialized = Json.Deserialize<DateTime[]>(serialized);
                assert.DeepEqual(deserialized, dates);
            });
        }
    }
}