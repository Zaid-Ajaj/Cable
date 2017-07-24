using Cable.Tests.Models;
using NUnit.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Tests
{
    [TestFixture]
    public class QueueTests
    {
        static WithQueue Sample()
        {
            var queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            return new WithQueue { Queue = queue };
        }


        [Test]
        public void QueueIsConvertedCorrectly()
        {
            var sample = Sample();

            var serialized = JsonConvert.SerializeObject(sample, new CableConverter());

            var deserialized = JsonConvert.DeserializeObject<WithQueue>(serialized, new CableConverter());

            Assert.AreEqual(sample.Queue.Dequeue(), deserialized.Queue.Dequeue());
            Assert.AreEqual(sample.Queue.Dequeue(), deserialized.Queue.Dequeue());
        }
    }
}
