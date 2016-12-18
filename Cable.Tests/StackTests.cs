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
    public class StackTests
    {
        static WithStack Sample()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            return new WithStack { Stack = stack };
        }

        [TestMethod]
        public void StackIsConvertedCorrectly()
        {
            var sample = Sample();

            var serialized = JsonConvert.SerializeObject(sample, new CableConverter());

            var deserialized = JsonConvert.DeserializeObject<WithStack>(serialized, new CableConverter());

            Assert.AreEqual(deserialized.Stack.Pop(), sample.Stack.Pop());
            Assert.AreEqual(deserialized.Stack.Pop(), sample.Stack.Pop());
            Assert.AreEqual(deserialized.Stack.Pop(), sample.Stack.Pop());
        }
    }
}
