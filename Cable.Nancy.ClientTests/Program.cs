using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Bridge.QUnit;
using Cable.Bridge;
using Client;

namespace Cable.Nancy.ClientTests
{

    public class Program
    {
        [Template("console.log({x})")]
        static extern void Log(object x);

        static void DatesEqual(Assert assert, DateTime x, DateTime y)
        {
            assert.Equal(x.Year, y.Year);
            assert.Equal(x.Month, y.Month);
            assert.Equal(x.Day, y.Day);
            assert.Equal(x.Hour, y.Hour);
            assert.Equal(x.Minute, y.Minute);
            assert.Equal(x.Second, y.Second);
            assert.Equal(x.Millisecond, y.Millisecond);
        }


        public static async void Main()
        {
            var Server = Bridge.Client.Resolve<IService>();

            QUnit.Module("Nancy Client Tests");
            QUnit.Test("Client is working", assert => assert.Equal(true, true));

            QUnit.Test("Client.Resolve<IService>() is defined", assert => assert.Equal(Script.IsDefined(Server), true));

            var stringResult = await Server.String("hello");
            QUnit.Test("IService.String()", assert => assert.Equal(stringResult, true));


            var now = new DateTime(2016, 12, 19, 20, 30, 0, 500);
            var objectsResult = await Server.StringIntCharDateTime("hello there", 5, 'a', now);
            QUnit.Test("IService.StringIntCharDateTime()", assert =>
            {
                assert.Equal(objectsResult.Length, 4);
                assert.Equal(objectsResult[0], "hello there");
                assert.Equal(objectsResult[1], 5);
                assert.Equal(objectsResult[2], 'a');

                var dateFromObjects = objectsResult[3].As<DateTime>();
                DatesEqual(assert, dateFromObjects, now);
            });


            //short int16 = 5;
            //ushort uint16 = 10;
            //int int32 = 20;
            //uint uint32 = 30;
            //long int64 = 35;
            //ulong uint64 = 40;
            //byte nByte = 45;
            //sbyte sByte = 50;
            //float Float = 2.4f;
            //double Double = 2.0;
            //decimal Decimal = 2.3454m;

            //var numericResults = await server.NumericPrimitives
            //(
            //   int16,
            //   uint16,
            //   int32,
            //   uint32,
            //   int64,
            //   uint64,
            //   nByte,
            //   sByte,
            //   Float,
            //   Double,
            //   Decimal
            //);


            //QUnit.Test("IService.NumericPrimitives()", assert =>
            //{
            //    assert.Equal(numericResults.Length, 11);
            //    assert.Equal(numericResults[0].As<short>() == int16, true);
            //    assert.Equal(numericResults[1].As<ushort>() == uint16, true);
            //    assert.Equal(numericResults[2].As<int>() == int32, true);
            //    assert.Equal(numericResults[3].As<uint>() == uint32, true);
            //    assert.Equal(numericResults[4].As<long>() == int64, true);
            //    assert.Equal(numericResults[5].As<ulong>() == uint64, true);
            //    assert.Equal(numericResults[6].As<byte>() == nByte, true);
            //    assert.Equal(numericResults[7].As<sbyte>() == sByte, true);
            //    assert.Equal(numericResults[8].As<float>() == Float, true);
            //    assert.Equal(numericResults[9].As<double>() == Double, true);
            //    assert.Equal(numericResults[10].As<decimal>() == Decimal, true);
            //});

            int[] arrayOfInts = { 1, 2, 3 };
            var resultArrayOfInts = await Server.ArrayOfInts(arrayOfInts);
            QUnit.Test("IService.ArrayofInts()", assert =>
            {
                for(var i = 0; i < arrayOfInts.Length; i++)
                {
                    assert.Equal(arrayOfInts[i], resultArrayOfInts[i]);
                }
            });

            string[] arrOfStringsReturningInts = { "1", "two", "three" };

            var intArrayResult = await Server.ArrayOfStringsReturningInts(arrOfStringsReturningInts);

            QUnit.Test("IService.ArrayOfStringsReturningInts()", assert =>
            {
                assert.Equal(intArrayResult[0], 1);
                assert.Equal(intArrayResult[1], 3);
                assert.Equal(intArrayResult[2], 5);
            });

            int[] ints1 = { 1, 2, 3 };
            string[] strings1 = { "", "hello" };
            char[] chars = { 'a', 'b', 'c', 'd' };
            var lengthsOfArrays = await Server.ArraysAsArguments(ints1, strings1, chars);

            QUnit.Test("IService.ArraysAsArguments()", assert =>
            {
                assert.Equal(lengthsOfArrays[0], 3);
                assert.Equal(lengthsOfArrays[1], 2);
                assert.Equal(lengthsOfArrays[2], 4);
            });

            var resultIntArrayNoArgs = await Server.NoArgumentsReturningIntArray();

            QUnit.Test("IService.NoArgumentsReturningIntArray()", assert =>
            {
                assert.Equal(resultIntArrayNoArgs[0], 1);
                assert.Equal(resultIntArrayNoArgs[1], 2);
                assert.Equal(resultIntArrayNoArgs[2], 3);
                assert.Equal(resultIntArrayNoArgs[3], 4);
                assert.Equal(resultIntArrayNoArgs[4], 5);
            });

            var resultStringNoArgs = await Server.NoArgumentsReturningString();

            QUnit.Test("IService.NoArgumentsReturningString()", assert =>
            {
                assert.Equal(resultStringNoArgs, "Result");
            });

            QUnit.Test("IService.SyncAdd()", assert =>
            {
                assert.Equal(Server.SyncAdd(1, 1), 2);
                assert.Equal(Server.SyncAdd(5, 6), 11);
                assert.Equal(Server.SyncAdd(2, 3), 5);
            });


            QUnit.Test("IService.NowPlusHoursSync()", assert =>
            {
                var result = Server.NowPlusHoursSync(5);
                assert.Equal(result.Hour == (DateTime.Now.AddHours(5).Hour), true);
            });

            var resultSumOfMatrixOfInt = await Server.SumOfMatrixOfInt(new int[][] 
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
                new int[] { 7, 8, 9 }
            });

            QUnit.Test("IService.SumOfMatrixOfInt()", assert =>
            {
                assert.Equal(resultSumOfMatrixOfInt, 45);
            });

            int[][] matrix =
            {
                 new int[] { 1, 2, 3 },
                 new int[] { 4, 5, 6 },
                 new int[] { 7, 8, 9 }
            };

            string[][] stringMatrix =
            {
                 new string[] { "Hello", "its" , "Me" }
            };

            var multipleMatrixElementCount = await Server.MatrixMultipleArgs(matrix, stringMatrix);

            QUnit.Test("IService.MatrixMultipleArgs()",  assert =>
            {
                assert.Equal(multipleMatrixElementCount, 12);
            });


            var longMatrix = new long[][]
            {
                new long[] { 1L, 2L, 3L, 4L, 5L }
            };

            var longMatrixSum = await Server.SumMatrixOfLong(longMatrix);

            QUnit.Test("IService.SumMatrixOfLong()", assert => assert.Equal(longMatrixSum == 15L, true));

            var date = new DateTime(1996, 11, 13, 0, 0, 0, 20);

            var person = new Person
            {
                Age = 20,
                Name = "Zaid",
                Money = 22.399m,
                IsMarried = false,
                DateOfBirth = date
            };

            QUnit.Test("IService.EchoPerson()", assert =>
            {
                var syncEchoedPerson = Server.EchoPerson(person);
                assert.Equal(syncEchoedPerson.Name, "Zaid");
                assert.Equal(syncEchoedPerson.Age, 20);
                assert.Equal(syncEchoedPerson.Money == 22.399m, true);
                DatesEqual(assert, date, syncEchoedPerson.DateOfBirth);
                assert.Equal(syncEchoedPerson.IsMarried, false);
            });

            var personTask = await Server.EchoTaskPerson(person);

            QUnit.Test("IService.EchoTaskPerson()", assert =>
            {
                assert.Equal(personTask.Name, "Zaid");
                assert.Equal(personTask.Age, 20);
                assert.Equal(personTask.Money == 22.399m, true);
                DatesEqual(assert, date, personTask.DateOfBirth);
                assert.Equal(personTask.IsMarried, false);
            });


            var myAge = await Server.HowOld(person);

            QUnit.Test("IService.HowOld()", assert => assert.Equal(myAge, 20));

            var numList = new List<int>();
            numList.AddRange(new int[] { 1, 2, 3, 4, 5 });

            var sumArray = await Server.IEnumerableSum(new int[] { 1, 2, 3, 4, 5 });
            var sumList = await Server.IEnumerableSum(numList);
            var sumRange = await Server.IEnumerableSum(Enumerable.Range(1, 5));

            QUnit.Test("IService.IEnumerableSum()", assert =>
            {
                assert.Equal(sumArray, 15);
                assert.Equal(sumList, 15);
                assert.Equal(sumRange, 15);
            });

            var genericInt = await Server.GenericInt(new Generic<int> { Value = 5 });
            QUnit.Test("IService.GenericInt()", assert =>
            {
                assert.Equal(genericInt, 5);
            });


            var simpleGeneric = await Server.GenericSimpleNested(new Generic<SimpleNested>
            {
                Value = new SimpleNested
                {
                    Int = 10,
                    String = "MyStr",
                    Long = 20L,
                    Double = 2.5,
                    Decimal = 3.5m
                }
            });

            QUnit.Test("IService.GenericSimpleNested()", assert => 
            {
                assert.Equal(simpleGeneric.Int, 10);
                assert.Equal(simpleGeneric.String, "MyStr");
                assert.Equal(simpleGeneric.Long == 20L, true);
                assert.Equal(simpleGeneric.Double, 2.5);
                assert.Equal(simpleGeneric.Decimal == 3.5m, true);
            });

            var genericPersonArg = new Generic<Person> { Value = person };

            var genericPerson = await Server.EchoGenericPerson(genericPersonArg);

            QUnit.Test("IService.EchoGenericPerson()", assert =>
            {
                assert.Equal(genericPerson.Value.Name, "Zaid");
                assert.Equal(genericPerson.Value.Age, 20);
                assert.Equal(genericPerson.Value.Money == 22.399m, true);
                DatesEqual(assert, date, genericPerson.Value.DateOfBirth);
                assert.Equal(genericPerson.Value.IsMarried, false);
                
            });

            var genericFstSnd = new DoubleGeneric<int, string>
            {
                First = 10,
                Second = "Zaid"
            };

            var resultFirst = await Server.ReturnFirst(genericFstSnd);
            var resultSecond = await Server.ReturnSecond(genericFstSnd);

            QUnit.Test("IService.ReturnFirst() and IService.ReturnSecond()", assert =>
            {
                assert.Equal(resultFirst, 10);
                assert.Equal(resultSecond, "Zaid");
            });

        }
    }
}
