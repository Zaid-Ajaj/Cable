using Bridge;
using Bridge.QUnit;
using System.Collections.Generic;
using System.Linq;

namespace Cable.Bridge.Tests
{
    public static class EnumerableTests
    {
        static IEnumerable<int> GetNumber(int x)
        {
            yield return x;
        }

        static void EnumerableIsConvertedCorrectly()
        {
            var sample = GetNumber(10);

            var encoded = Converters.EncodeObject(sample);

            QUnit.Test("IEnumerable with yield return is converted corretly to array", assert =>
            {
                assert.Equal(encoded == null, false);
                assert.Equal(encoded["Type"], "Array");
                assert.Equal(encoded["Value"]["length"], 1);

                var number = Script.Write<object>("encoded.Value[0]");
                assert.Equal(number["Type"], "Int32");
                assert.Equal(number["Value"], 10);
            });

            QUnit.Test("IEnumerable from range is converted correctly", assert =>
            {
                var range = System.Linq.Enumerable.Range(1, 1);
                var converted = Converters.EncodeObject(range);
                assert.Equal(converted == null, false);
                assert.Equal(converted["Type"], "Array");
                assert.Equal(converted["Value"]["length"], 1);

                var number = Script.Write<object>("converted.Value[0]");
                assert.Equal(number["Type"], "Int32");
                assert.Equal(number["Value"], 1);
            });

            QUnit.Test("IEnumerable of int is serialized and deserialized correctly", assert =>
            {
                IEnumerable<int> enu = Enumerable.Range(1, 10);
                var serialized = Json.Serialize(enu);
                var deserialized = Json.Deserialize<IEnumerable<int>>(serialized);
                assert.Equal(deserialized.Count(), enu.Count());

                var enuArr = enu.ToArray();
                var deserializedArr = deserialized.ToArray();

                for(int i = 0; i < enuArr.Length; i++)
                {
                    assert.Equal(enuArr[i], deserializedArr[i]);
                }
            });

        }
        public static void Run()
        {
            QUnit.Module("Enumerable tests");
            EnumerableIsConvertedCorrectly();
        }
    }
}