using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Cable.Console
{
    class Nested
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }

    class ClassWithDateTime
    {
        public DateTime Birthday { get; set; }
        public long Id { get; set; }
        public Simple Nested { get; set; }
    }

    class Simple
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<int> Ints { get; set; }
    }

    class SuperSimple { public string Str { get; set; } }

    class Program
    {
        static void Main(string[] args)
        {
            var arr = new object[] { "", 1m, "string" };

            var serialized = Json.Serialize(arr);

            var deserialized = Json.Deserialize<object[]>(serialized);

            System.Console.ReadKey();
        }
    }

    public class SimpleEnum { public Choices Choice { get; set; } }
    public enum Choices { One, Two }
}
