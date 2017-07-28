using Bridge;
using Bridge.QUnit;
using System.Collections.Generic;
using System.Linq;

namespace Cable.Bridge.Tests
{
    public static class ListTests
    {

        public static void Run()
        {
            QUnit.Module("List Tests");


            QUnit.Test("List is encoded correctly", assert =>
            {
                var list = new List<int>();
                list.Add(1);
                list.Add(2);
                list.Add(3);

                var encoded = Converters.EncodeObject(list);

                assert.Equal(encoded["Type"], "List");
                assert.Equal(encoded["Value"]["length"], list.Count);
            });


            QUnit.Test("Serilization and deserialization of List<int> works", assert =>
            {
                var sample = new List<int>();
                sample.Add(10);
                sample.Add(20);

                var serialized = Json.Serialize(sample);
                var deserialized = Json.Deserialize<List<int>>(serialized);

                assert.Equal(deserialized == null, false);
                assert.Equal(sample.Count, deserialized.Count);

                for(int i = 0; i < sample.Count; i++)
                {
                    assert.Equal(sample[i], deserialized[i]);
                }
                

                deserialized.Add(30);
                assert.Equal(deserialized.Sum(), 60);
                
            });



        }
    }
}