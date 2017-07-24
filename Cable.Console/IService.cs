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

        Task<object[]> NumericPrimitives(int Int, long Int64, double Double, decimal Decimal);

        Task<int[]> ArrayOfInts(int[] arr);

        Task<string> EchoNullString();

        Task<double> EchoDouble(double x);

        Task<WrappedDouble> EchoWrappedDouble(WrappedDouble x);

        Task<int[]> ArrayOfStringsReturningInts(string[] arr);

        Task<int[]> ArraysAsArguments(int[] ints, string[] strings, char[] chars);

        Task<DateTime[]> ArrayOfDateTime(DateTime[] dates);

        Task<int[]> NoArgumentsReturningIntArray();

        Task<string> NoArgumentsReturningString();

        Task<List<int>> EchoListOfInt(List<int> input);

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

    }
}
