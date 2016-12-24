using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;

namespace Cable.Console
{
    public class Service : IService
    {
        public Task<bool> String(string input)
        {
            bool result = true;
            if (string.IsNullOrEmpty(input))
            {
                result = false;
            }
            return Task.FromResult(result);
        }

        public Task<bool> StringInt(string arg1, int arg2)
        {
            bool result = true;
            if (string.IsNullOrEmpty(arg1) || arg2 == default(int))
            {
                result = false;
            }
            return Task.FromResult(result);
        }

        public Task<object[]> StringIntCharDateTime(string arg1, int arg2, char arg3, DateTime dt)
        {
            var result = new object[] { arg1, arg2, arg3, dt };
            return Task.FromResult(result);
        }


        public Task<object[]> NumericPrimitives
        (
            short Int16,
            ushort UInt16,
            int Int,
            uint UInt,
            long Int64,
            ulong UInt64,
            byte Byte,
            sbyte SByte,
            float Float,
            double Double,
            decimal Decimal
        )
        {
            var result = new object[] { Int16, UInt16, Int, UInt, Int64, UInt64, Byte, SByte, Float, Double, Decimal };
            return Task.FromResult(result);
        }

        public Task<DateTime[]> ArrayOfDateTime(DateTime[] dates)
        {
            return Task.FromResult(dates);
        }

        public Task<int[]> ArrayOfInts(int[] arr)
        {
            return Task.FromResult(arr);
        }

        public Task<int[]> ArrayOfStringsReturningInts(string[] arr)
        {
            var ints = arr.Select(str => str.Length).ToArray();
            return Task.FromResult(ints);
        }

        public Task<int[]> ArraysAsArguments(int[] ints, string[] strings, char[] chars)
        {
            int[] result = { ints.Length, strings.Length, chars.Length };
            return Task.FromResult(result);
        }


        public Task<int[]> NoArgumentsReturningIntArray()
        {
            var result = new int[] { 1, 2, 3, 4, 5 };
            return Task.FromResult(result);
        }

        public Task<string> NoArgumentsReturningString()
        {
            return Task.FromResult("Result");
        }

        public int SyncAdd(int x, int y) => x + y;

        public DateTime NowPlusHoursSync(int n)
        {
            return DateTime.Now.AddHours(n);
        }

        public int Sum(int[] numbers)
        {
            return numbers.Sum();
        }

        public string SyncNoArguments()
        {
            return "SyncString";
        }

        public int DifferenceInMinutes(DateTime from, DateTime to)
        {
            var span = TimeSpan.FromTicks(Math.Abs(from.Ticks - to.Ticks));
            return span.Minutes;
        }

        public Task<int> SumOfMatrixOfInt(int[][] ints)
        {
            int result = ints.Select(innerInts => innerInts.Sum()).Sum();
            return Task.FromResult(result);
        }

        public Task<int> MatrixMultipleArgs(int[][] ints, string[][] strs)
        {
            var intsCount = ints.Select(xs => xs.Length).Sum();
            var stringsCount = strs.Select(xs => xs.Length).Sum();
            var result = intsCount + stringsCount;
            return Task.FromResult(result);
        }

        public Task<long> SumMatrixOfLong(long[][] longs)
        {
            var result = longs.SelectMany(xs => xs).Sum();
            return Task.FromResult(result);
        }

        public Person EchoPerson(Person p) => p;

        public Task<Person> EchoTaskPerson(Person p) => Task.FromResult(p);

        public Task<int> HowOld(Person p)
        {
            var span = TimeSpan.FromTicks(DateTime.Now.Ticks - p.DateOfBirth.Ticks);
            var years = span.TotalDays / 365.25;
            return Task.FromResult((int)years);
        }

        public Task<int> IEnumerableSum(IEnumerable<int> xs)
        {
            return Task.FromResult(xs.Sum());
        }

        public Task<int> GenericInt(Generic<int> g) => Task.FromResult(g.Value);

        public Task<SimpleNested> GenericSimpleNested(Generic<SimpleNested> g) => Task.FromResult(g.Value);

        public Task<Generic<Person>> EchoGenericPerson(Generic<Person> g)
        {
            return Task.FromResult(g);
        }

        public Task<int> ReturnFirst(DoubleGeneric<int, string> g) => Task.FromResult(g.First);

        public Task<string> ReturnSecond(DoubleGeneric<int, string> g) => Task.FromResult(g.Second);

    }
}
