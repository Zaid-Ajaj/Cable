using Cable.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Tests
{
    [TestClass]
    public class ArrayTests
    {
        static WithArray Sample() => new WithArray
        {
            Array = new int[] { 1, 2, 3 }
        };

        [TestMethod]
        public void ArrayIsConvertedCorrectly()
        {
            var sample = Sample();

            var serialized = JsonConvert.SerializeObject(sample, Formatting.Indented, new CableConverter());

            var deserialized = JsonConvert.DeserializeObject<WithArray>(serialized, new CableConverter());

            Assert.AreEqual(sample.Array.Length, deserialized.Array.Length);

            for(int i = 0; i < sample.Array.Length; i++)
            {
                Assert.AreEqual(sample.Array[i], deserialized.Array[i]);
            }
        }


        [TestMethod]
        public void BoxedArrayIsConvertedCorrectly()
        {
            var arr = new int[] { 1, 2, 3 };

            var serialized = Json.Serialize(arr);

            dynamic deserialized = Json.Deserialize<int[]>(serialized);

            Assert.AreEqual(arr[0], deserialized[0]);
            Assert.AreEqual(arr[1], deserialized[1]);
            Assert.AreEqual(arr[2], deserialized[2]);
        }


        [TestMethod]
        public void ArrayOfArrayIsConvertedCorrectly()
        {
            var sample = new int[][]
            {
                new int[] {1,2,3},
                new int[] {3,4,5}
            };

            var serialized = Json.Serialize(sample);

            var deserialized = Json.Deserialize<int[][]>(serialized);

            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(sample[i][j], deserialized[i][j]);
                }
            }
        }

        [TestMethod]
        public void ArrayOfObjectsConvertedCorrectly()
        {
            var objs = new object[] { 5L, "hello" };

            var serialized = Json.Serialize(objs);

            var deserialized = Json.Deserialize<object[]>(serialized);

            Assert.AreEqual(objs[0], deserialized[0]);
            Assert.AreEqual(objs[1], deserialized[1]);
        }
    }
}
