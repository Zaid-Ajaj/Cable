using NUnit.Framework;
using Nancy.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cable.Nancy.Tests
{
    [TestFixture]
    public class IServiceTests
    {
        static Bootstraper bootstrapper = new Bootstraper();

        static string Args(object[] args)
        {
            return Json.Serialize(args);
        }

        static T FromBody<T>(BrowserResponseBodyWrapper body)
        {
            return Json.Deserialize<T>(body.AsString());
        }

        void SendReq(string path, params object[] args)
        {
            var browser = new Browser(bootstrapper);

            var response = browser.Post(path, ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(args));
            });

            var objs = FromBody<object[]>(response.Body);

            for(int i = 0; i < args.Length; i++)
            {
                var converted = objs[i];
                if (converted.GetType() == typeof(DateTime))
                {
                    var time = (DateTime)converted;
                    var original = (DateTime)args[i];

                    Assert.That(time, Is.EqualTo(original).Within(TimeSpan.FromMilliseconds(10)));
                }
                else
                {
                    Assert.That(objs[i], Is.EqualTo(args[i]));
                }
            }
        }

        [Test]
        public void SingleArgumentOfString()
        {
            var browser = new Browser(bootstrapper);

            var response = browser.Post("/IService/String", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { "hello" }));
            });

            Assert.AreEqual(true, FromBody<bool>(response.Body));
        }

        [Test]
        public void MultipleArgumentsAreMappedCorrectly()
        {
            SendReq("/IService/StringIntCharDateTime", "hello there", 5, 'a', DateTime.Now);
        }

        [Test]
        public void NumericPrimitivesAreCorrectlyPassedToMethod()
        {
            short int16 = 5;
            ushort uint16 = 10;
            int int32 = 20;
            uint uint32 = 30;
            long int64 = 35;
            ulong uint64 = 40;
            byte nByte = 45;
            sbyte sByte = 50;
            float Float = 2.45f;
            double Double = 2.0;
            decimal Decimal = 2.3454m;

            object[] numericValues = 
            {
                int16, uint16, int32, uint32,
                int64, uint64, nByte, sByte,
                Float, Double, Decimal
            };

            SendReq("/IService/NumericPrimitives", numericValues);
        }


        [Test]
        public void ArrayOfInts()
        {
            var browser = new Browser(bootstrapper);

            int[] ints = { 1, 2, 3 };
            var response = browser.Post("/IService/ArrayOfInts", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { ints }));
            });

            var result = FromBody<int[]>(response.Body);

            for(int i = 0; i < ints.Length; i++)
            {
                Assert.AreEqual(result[i], ints[i]);
            }
        }

        [Test]
        public void ArrayOfStringsReturningInts()
        {
            var browser = new Browser(bootstrapper);

            string[] strings = { "1", "two", "three" };

            var response = browser.Post("/IService/ArrayOfStringsReturningInts", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { strings }));
            });

            var result = FromBody<int[]>(response.Body);

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(3, result[1]);
            Assert.AreEqual(5, result[2]);
        }

        [Test]
        public void ArraysAsArguments()
        {
            var browser = new Browser(bootstrapper);

            int[] ints = { 1, 2, 3, 4, 5 };
            string[] strings = { "one", "two", "three" };
            char[] chars = { 'a' };

            var response = browser.Post("/IService/ArraysAsArguments", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { ints, strings, chars }));
            });

            var result = FromBody<int[]>(response.Body);

            Assert.AreEqual(5, result[0]);
            Assert.AreEqual(3, result[1]);
            Assert.AreEqual(1, result[2]);
        }

        [Test]
        public void ArrayOfDateTime()
        {
            DateTime[] dates = { DateTime.Now, DateTime.Now.AddMinutes(30) };

            var browser = new Browser(bootstrapper);

            var res = browser.Post("/IService/ArrayOfDateTime", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { dates }));
            });

            var result = FromBody<DateTime[]>(res.Body);

            for(int i = 0; i < dates.Length; i++)
            {
                var time = result[i];
                var original = dates[i];

                Assert.That(time, Is.EqualTo(original).Within(TimeSpan.FromMilliseconds(5)));
            }
        }


        [Test]
        public void NoArgumentsReturningIntArray()
        {
            var browser = new Browser(bootstrapper);

            var res = browser.Post("/IService/NoArgumentsReturningIntArray", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { }));
            });

            var result = FromBody<int[]>(res.Body);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.That(i + 1, Is.EqualTo(result[i]));
            }
        }

        [Test]
        public void NoArgumentsReturningString()
        {
            var browser = new Browser(bootstrapper);

            var res = browser.Post("/IService/NoArgumentsReturningString", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { }));
            });

            var result = FromBody<string>(res.Body);

            Assert.AreEqual("Result", result);
        }

        [Test]
        public void SyncAdd()
        {
            var browser = new Browser(bootstrapper);

            var res = browser.Post("/IService/SyncAdd", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { 5, 6 }));
            });

            var result = FromBody<int>(res.Body);

            Assert.AreEqual(11, result);
        }

        [Test]
        public void NowPlusHoursSync()
        {
            var browser = new Browser(bootstrapper);

            
            var res = browser.Post("/IService/NowPlusHoursSync", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { 5 }));
            });

            var result = FromBody<DateTime>(res.Body);

            Assert.That(DateTime.Now.AddHours(5), Is.EqualTo(result).Within(TimeSpan.FromSeconds(2)));
        }

        [Test]
        public void Sum()
        {
            var browser = new Browser(bootstrapper);

            int[] numbers = Enumerable.Range(1, 10).ToArray();

            var res = browser.Post("/IService/Sum", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { numbers }));
            });

            var sum = FromBody<int>(res.Body);

            Assert.That(sum, Is.EqualTo(55));
        }

        [Test]
        public void SyncNoArguments()
        {
            var browser = new Browser(bootstrapper);

            var res = browser.Post("/IService/SyncNoArguments", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { }));
            });

            var result = FromBody<string>(res.Body);

            Assert.That(result, Is.EqualTo("SyncString"));
        }

        [Test]
        public void DifferenceInMinutes()
        {
            var browser = new Browser(bootstrapper);

            var from = DateTime.Now;

            var to = from.AddMinutes(10);

            var res = browser.Post("/IService/DifferenceInMinutes", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { from , to }));
            });

            var result = FromBody<int>(res.Body);

            Assert.That(result, Is.EqualTo(10));
        }


        [Test]
        public void SumOfMatrixOfInt()
        {
            var browser = new Browser(bootstrapper);

            int[][] matrix =
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
                new int[] { 7, 8, 9 }
            };

            var res = browser.Post("/IService/SumOfMatrixOfInt", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { matrix }));
            });

            var result = FromBody<int>(res.Body);

            Assert.That(result, Is.EqualTo(45));
        }

        [Test]
        public void MatrixMultipleArgs()
        {
            var browser = new Browser(bootstrapper);

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

            var res = browser.Post("/IService/MatrixMultipleArgs", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { matrix, stringMatrix }));
            });

            var result = FromBody<int>(res.Body);

            Assert.That(result, Is.EqualTo(12));
        }


        [Test]
        public void SumMatrixOfLong()
        {
            var browser = new Browser(bootstrapper);

            long[][] matrix =
            {
                 new long[] { 1L, 2L, 3L },
                 new long[] { 4L, 5L, 6L },
                 new long[] { 7L, 8L, 9L }
            };

            var res = browser.Post("/IService/SumMatrixOfLong", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { matrix }));
            });

            var result = FromBody<long>(res.Body);

            Assert.That(result, Is.EqualTo(45L));
        }

        bool AllEqual<T>(params T[] ts) where T : IComparable<T>
        {
            return ts.All(t => ts.All(tPrime => t.CompareTo(tPrime) == 0));
        }

        [Test]
        public void AllEqualTest()
        {
            Assert.AreEqual(true, AllEqual(1, 1, 1, 1, 1));
            Assert.AreEqual(true, AllEqual("", "", ""));
            Assert.AreEqual(false, AllEqual(1, 2, 3));
        }

        [Test]
        public void Person()
        {
            var browser = new Browser(bootstrapper);

            var birthdate = new DateTime(1996, 11, 13);

            var person = new Person
            {
                Name = "Zaid",
                Money = 22.0m,
                IsMarried = false,
                Gender = Gender.Male,
                DateOfBirth = birthdate
            };

            var echoedSync = browser.Post("/IService/EchoPerson", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { person }));
            });

            var echoedAsync = browser.Post("/IService/EchoTaskPerson", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { person }));
            });

            var howOld = browser.Post("/IService/HowOld", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { person }));
            });

            var echoSync = FromBody<Person>(echoedSync.Body);
            var echoAsync = FromBody<Person>(echoedAsync.Body);
            var years = FromBody<int>(howOld.Body);

            Assert.AreEqual(true, AllEqual(echoSync.Name, echoAsync.Name, person.Name));
            Assert.AreEqual(true, AllEqual(echoSync.Money, echoAsync.Money, person.Money));
            Assert.AreEqual(true, AllEqual(echoSync.IsMarried, echoAsync.IsMarried, person.IsMarried));
            Assert.AreEqual(true, AllEqual((int)echoSync.Gender, (int)echoAsync.Gender, (int)person.Gender));
            Assert.AreEqual(true, AllEqual(echoSync.DateOfBirth, echoAsync.DateOfBirth, person.DateOfBirth, birthdate));
            Assert.AreEqual(years, (int)(TimeSpan.FromTicks(DateTime.Now.Ticks - birthdate.Ticks).TotalDays / 365.25));
        }

        [Test]
        public void IEnumerableSumFromRange()
        {
            var browser = new Browser(bootstrapper);

            var res = browser.Post("/IService/IEnumerableSum", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { Enumerable.Range(1, 10) }));
            });

            var result = FromBody<int>(res.Body);

            Assert.That(result, Is.EqualTo(55));
        }

        [Test]
        public void IEnumerableSumFromArray()
        {
            var browser = new Browser(bootstrapper);

            var res = browser.Post("/IService/IEnumerableSum", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { Enumerable.Range(1, 10).ToArray() }));
            });

            var result = FromBody<int>(res.Body);

            Assert.That(result, Is.EqualTo(55));
        }

        [Test]
        public void IEnumerableSumFromList()
        {
            var browser = new Browser(bootstrapper);

            var res = browser.Post("/IService/IEnumerableSum", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { Enumerable.Range(1, 10).ToList() }));
            });

            var result = FromBody<int>(res.Body);

            Assert.That(result, Is.EqualTo(55));
        }

        [Test]
        public void EchoGenericPerson()
        {
            var browser = new Browser(bootstrapper);

            var generic = new Generic<Person>
            {
                Value = new Person
                {
                    Name = "Zaid",
                    Money = 22.0m,
                    IsMarried = false,
                    Gender = Gender.Male,
                }
            };

            var res = browser.Post("/IService/EchoGenericPerson", ctx =>
            {
                ctx.AjaxRequest();
                ctx.Body(Args(new object[] { generic }));
            });

            var result = FromBody<Generic<Person>>(res.Body);

            Assert.That(result.Value.Name, Is.EqualTo("Zaid"));
            Assert.That(result.Value.Gender, Is.EqualTo(Gender.Male));
            Assert.That(result.Value.Money, Is.EqualTo(22.0m));
            Assert.That(result.Value.IsMarried, Is.EqualTo(false));
        }
    }
}
