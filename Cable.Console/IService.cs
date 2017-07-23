using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public interface IService
    {
        Task<bool> String(string input);

        Task<WrappedDateTime> EchoWrappedDateTime(WrappedDateTime data);

        Task<object[]> StringIntCharDateTime(string arg1, int arg2, char arg3, DateTime date);

        Task<object[]> NumericPrimitives
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
        );

        Task<int[]> ArrayOfInts(int[] arr);

        Task<int[]> ArrayOfStringsReturningInts(string[] arr);

        Task<int[]> ArraysAsArguments(int[] ints, string[] strings, char[] chars);

        Task<DateTime[]> ArrayOfDateTime(DateTime[] dates);

        Task<int[]> NoArgumentsReturningIntArray();

        Task<string> NoArgumentsReturningString();

        int SyncAdd(int x, int y);

        DateTime NowPlusHoursSync(int n);

        int Sum(int[] numbers);

        string SyncNoArguments();

        int DifferenceInMinutes(DateTime from, DateTime to);

        Task<int> SumOfMatrixOfInt(int[][] ints);

        Task<int> MatrixMultipleArgs(int[][] ints, string[][] strs);

        Task<long> SumMatrixOfLong(long[][] longs);

        Person EchoPerson(Person p);

        Task<Person> EchoTaskPerson(Person p);

        Task<int> HowOld(Person p);

        Task<int> IEnumerableSum(IEnumerable<int> xs);

        Task<Generic<Person>> EchoGenericPerson(Generic<Person> g);

        Task<int> GenericInt(Generic<int> g);

        Task<SimpleNested> GenericSimpleNested(Generic<SimpleNested> g);

        Task<int> ReturnFirst(DoubleGeneric<int, string> g);

        Task<string> ReturnSecond(DoubleGeneric<int, string> g);

        //Task<bool> ArrayOfDecimals(decimal[] decimals);

        //Task<bool> ArrayOfStrings(string[] strings);

        //Task<bool> ArrayOfObjects(object[] objects);

        //Task<bool> ArrayOfArrays(object[][] matrix);
    }
}
