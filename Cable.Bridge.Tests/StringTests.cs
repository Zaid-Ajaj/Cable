using Bridge;
using Bridge.QUnit;

namespace Cable.Bridge.Tests
{
    public static class StringTests
    {
        public static void Run()
        {
            QUnit.Module("String tests");

            var sample = "my string";

            QUnit.Test("Strings is serialized and deserialized correctly", assert =>
            {
                var serialized = Json.Serialize(sample);

                var deserialized = Json.Deserialize<string>(serialized);

                assert.Equal(deserialized, sample);
            });
        }
    }
}