using Cable.Tests.Models;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cable.Tests
{
    [TestFixture]
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

        [Test]
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
