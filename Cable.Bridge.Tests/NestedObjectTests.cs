using Bridge;
using Bridge.QUnit;
using System;

namespace Cable.Bridge.Tests
{
    

    public class Nested<T>
    {
        public T InnerInner { get; set; }
    }

    public class WeirdGenericClass<T>
    {
        public Nested<T> Nested { get; set; }
        public T Inner { get; set; }
    }

    public static class NestedObjectTests
    {
        public class WrappedInt
        {
            public int Id { get; set; }
            public long Large { get; set; }
            public string Name { get; set; }
            public bool HasValue { get; set; }
        }

        public static void Run()
        {
            QUnit.Module("Nested object tests");

            var sample = new WrappedInt
            {
                Id = 5,
                Name = "Zaid",
                HasValue = true,
                Large = 238472746327434243L
            };

            QUnit.Test("Nested object is encoded correctly", assert =>
            {
                var encoded = Converters.EncodeObject(sample);
                assert.Equal(encoded["IsPrimitive"], false);
                assert.Equal(encoded["IsArray"], false);
                assert.Equal(encoded["IsNumeric"], false);
                assert.Equal(encoded["Type"], "Cable.Bridge.Tests.NestedObjectTests.WrappedInt");
                assert.Equal(encoded["Value"]["Id"] == null, false);
                assert.Equal(encoded["Value"]["Id"]["IsPrimitive"], true);
                assert.Equal(encoded["Value"]["Id"]["IsArray"], false);
                assert.Equal(encoded["Value"]["Id"]["IsNumeric"], true);
                assert.Equal(encoded["Value"]["Id"]["Type"], "Int32");
                assert.Equal(encoded["Value"]["Id"]["Value"], "5");
                assert.Equal(encoded["Value"]["Name"] == null, false);
                assert.Equal(encoded["Value"]["Name"]["IsPrimitive"], true);
                assert.Equal(encoded["Value"]["Name"]["IsArray"], false);
                assert.Equal(encoded["Value"]["Name"]["IsNumeric"], false);
                assert.Equal(encoded["Value"]["Name"]["Type"], "String");
                assert.Equal(encoded["Value"]["Name"]["Value"], "Zaid");
                assert.Equal(encoded["Value"]["HasValue"] == null, false);
                assert.Equal(encoded["Value"]["HasValue"]["IsPrimitive"], true);
                assert.Equal(encoded["Value"]["HasValue"]["IsArray"], false);
                assert.Equal(encoded["Value"]["HasValue"]["IsNumeric"], false);
                assert.Equal(encoded["Value"]["HasValue"]["Type"], "Boolean");
                assert.Equal(encoded["Value"]["HasValue"]["Value"], true);
                assert.Equal(encoded["Value"]["Large"] == null, false);
                assert.Equal(encoded["Value"]["Large"]["IsPrimitive"], true);
                assert.Equal(encoded["Value"]["Large"]["IsArray"], false);
                assert.Equal(encoded["Value"]["Large"]["IsNumeric"], true);
                assert.Equal(encoded["Value"]["Large"]["Type"], "Int64");
                assert.Equal(encoded["Value"]["Large"]["Value"], "238472746327434243");
            });

            QUnit.Test("Serialization and deserialization of nested object works", assert =>
            {
                var serialized = Json.Serialize(sample);

                var deserialized = Json.Deserialize<WrappedInt>(serialized);

                assert.Equal(sample.Id, deserialized.Id);
                assert.Equal(sample.Name, deserialized.Name);
                assert.Equal(sample.HasValue, deserialized.HasValue);
                assert.Equal(sample.Large == deserialized.Large, true);
            });

            var generic = new WeirdGenericClass<WeirdGenericClass<int>>
            {
                Inner = new WeirdGenericClass<int>
                {
                    Inner = 20,
                    Nested = new Nested<int>
                    {
                        InnerInner = 5
                    }
                },
                Nested = new Nested<WeirdGenericClass<int>>
                {
                    InnerInner = new WeirdGenericClass<int>
                    {
                        Inner = 30,
                        Nested = new Nested<int>
                        {
                            InnerInner = 40
                        }
                    }
                }
            };



            QUnit.Test("Weird generic class in converted correctly", assert =>
            {
               

                var serialized = Json.Serialize(generic);

                var deserialized = Json.Deserialize<WeirdGenericClass<WeirdGenericClass<int>>>(serialized);
                

                assert.Equal(generic.Inner.Inner, deserialized.Inner.Inner);
                assert.Equal(generic.Inner.Nested.InnerInner, deserialized.Inner.Nested.InnerInner);
                assert.Equal(generic.Nested.InnerInner.Inner, deserialized.Nested.InnerInner.Inner);
                assert.Equal(generic.Nested.InnerInner.Nested.InnerInner, deserialized.Nested.InnerInner.Nested.InnerInner);

            }); 
        }
    }
}