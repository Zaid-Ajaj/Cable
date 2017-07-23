using System;


namespace Client
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Money { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMarried { get; set; }
    }

    public class WrappedDateTime
    {
        public DateTime Value { get; set; }
    }

    public class WrappedDouble
    {
        public double Value { get; set; }
    }

    public class SimpleNested
    {
        public int Int { get; set; }
        public string String { get; set; }
        public long Long { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
    }

    public class Generic<T>
    {
        public T Value { get; set; }
    }


    public class DoubleGeneric<T, U>
    {
        public T First { get; set; }
        public U Second { get; set; }
    }
}
