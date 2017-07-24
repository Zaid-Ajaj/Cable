using Bridge;
using Bridge.QUnit;
using System;

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

                var deserialized = Json.Deserialize(serialized, typeof(string)).As<string>();

                assert.Equal(deserialized, sample);
            });
        }
    }
}