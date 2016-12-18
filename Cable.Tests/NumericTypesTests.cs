using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cable.Tests.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cable.Tests
{
    [TestClass]
    public class NumericTypesTests
    {
        private static NumericProps Sample() => new NumericProps
        {
            Byte = 155,
            Decimal = 10m,
            Double = 2.0,
            Float = 20f,
            Int = 10,
            Int16 = 10,
            UInt = 42u,
            Int64 = 3525621412421,
            SByte = 100,
            UInt16 = 10,
            UInt64 = 2341434141242u

        };


        [TestMethod]
        public void NumberTypesAreConvertedCorrectly()
        {
            var sample = Sample();

            var serialized = JsonConvert.SerializeObject(sample, Formatting.Indented, new CableConverter());

            dynamic deserialized = JsonConvert.DeserializeObject<NumericProps>(serialized, new CableConverter());

            Assert.AreEqual(deserialized.Double.GetType(), typeof(double));
            Assert.AreEqual(deserialized.Int.GetType(), typeof(int));
            Assert.AreEqual(deserialized.Float.GetType(), typeof(float));
            Assert.AreEqual(deserialized.Int64.GetType(), typeof(long));
            Assert.AreEqual(deserialized.UInt64.GetType(), typeof(ulong));
            Assert.AreEqual(deserialized.UInt.GetType(), typeof(uint));
            Assert.AreEqual(deserialized.Byte.GetType(), typeof(byte));
            Assert.AreEqual(deserialized.SByte.GetType(), typeof(sbyte));
            Assert.AreEqual(deserialized.Int16.GetType(), typeof(short));
            Assert.AreEqual(deserialized.UInt16.GetType(), typeof(ushort));
            Assert.AreEqual(deserialized.Decimal.GetType(), typeof(decimal));
        }

        [TestMethod]
        public void NumericValuesAreConvertedCorrectly()
        {
            var sample = Sample();

            var serialized = Json.Serialize(sample);

            var deserialized = Json.Deserialize<NumericProps>(serialized);

            Assert.AreEqual(sample.Byte, deserialized.Byte);
            Assert.AreEqual(sample.Decimal, deserialized.Decimal);
            Assert.AreEqual(sample.Int, deserialized.Int);
            Assert.AreEqual(sample.UInt, deserialized.UInt);
            Assert.AreEqual(sample.UInt16, deserialized.UInt16);
            Assert.AreEqual(sample.Int16, deserialized.Int16);
            Assert.AreEqual(sample.Double, deserialized.Double);
            Assert.AreEqual(sample.Float, deserialized.Float);
            Assert.AreEqual(sample.UInt64, deserialized.UInt64);
            Assert.AreEqual(sample.Int64, deserialized.Int64);
        }
    }
}
