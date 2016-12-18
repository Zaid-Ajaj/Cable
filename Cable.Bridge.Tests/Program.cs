using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Bridge.Tests
{
    public class Program
    {
        public static void Main()
        {
            PropertyExtractionTests.Run();
            DateTimeTests.Run();
            ArrayTests.Run();
            ListTests.Run();
            EnumerableTests.Run();
            NestedObjectTests.Run();
            StringTests.Run();
            NumericTypesTests.Run();
            InheritanceTests.Run();
            ServiceRegistrationTests.Run();
            TimeSpanTests.Run();
        }
    }
}
