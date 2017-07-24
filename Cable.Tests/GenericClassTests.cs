using Cable.Tests.Models;
using NUnit.Framework;
using Newtonsoft.Json;



namespace Cable.Tests
{
    [TestFixture]
    public class GenericClassTests
    {
        [Test]
        public void GenericNesteClassIsConvertedCorrectly()
        {
            var sample = new GenericClass<NestedInGeneric<int>>
            {
                Value = new NestedInGeneric<int> { InnerInner = 1 },
                OtherValue = new NestedInGeneric<NestedInGeneric<int>>
                {
                    InnerInner = new NestedInGeneric<int>
                    {
                        InnerInner = 5
                    }
                }
            };

            var serialized = JsonConvert.SerializeObject(sample, Formatting.Indented, new CableConverter());

            var deserialized = JsonConvert.DeserializeObject<GenericClass<NestedInGeneric<int>>>(serialized, new CableConverter());

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(deserialized.Value.InnerInner, sample.Value.InnerInner);
            Assert.AreEqual(deserialized.OtherValue.InnerInner.InnerInner, sample.OtherValue.InnerInner.InnerInner);


        }
    }
}
