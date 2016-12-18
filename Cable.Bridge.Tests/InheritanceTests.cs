using Bridge;
using Bridge.QUnit;
using System;

namespace Cable.Bridge.Tests
{
    public abstract class Base
    {
        public int Id { get; set; }
        public long Long { get; set; }
    }

    public class Derived : Base
    {
        public DateTime DateTime { get; set; }
        public decimal Decimal { get; set; }
    }


    public abstract class BaseGeneric<T>
    {
        public T Value { get; set; }
    }

    public class GenericDerived<T, U> : BaseGeneric<U>
    {
        public T OtherValue { get; set; }
    }

    public class DerivedFromGeneric : BaseGeneric<int> { }

    public static class InheritanceTests
    {

        public static void Run()
        {
            QUnit.Module("Inheritance Tests");

            QUnit.Test("Inherited class from abstract class is converted correctly", assert =>
            {
                var sample = new Derived
                {
                    Id = 5,
                    Long = 10L,
                    Decimal = 10m,
                    DateTime = DateTime.Now
                };

                var serialized = Json.Serialize(sample);

                var deserialized = Json.Deserialize<Derived>(serialized);

                assert.Equal(deserialized.Id, sample.Id);
                assert.Equal(deserialized.DateTime == sample.DateTime, true);
                assert.Equal(deserialized.Decimal == sample.Decimal, true);
                assert.Equal(deserialized.Long == sample.Long, true);

            });

            QUnit.Test("Inherited class from generic abstract class is converted correctly", assert =>
            {
                var sample = new DerivedFromGeneric
                {
                    Value = 20
                };

                var serialized = Json.Serialize(sample);

                var deserialized = Json.Deserialize<DerivedFromGeneric>(serialized);

                assert.Equal(deserialized.Value, 20);
            });

            QUnit.Test("Inherited generic class from generic abstract class is converted correctly", assert =>
            {
                var sample = new GenericDerived<int, string>
                {
                    Value = "MyValue",
                    OtherValue = 10
                };

                var serialized = Json.Serialize(sample);

                var deserialized = Json.Deserialize<GenericDerived<int, string>>(serialized);

                assert.Equal(deserialized.Value, sample.Value);

                assert.Equal(deserialized.OtherValue, sample.OtherValue);

            });
        }
    }
}