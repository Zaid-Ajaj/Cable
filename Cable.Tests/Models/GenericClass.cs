using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Tests.Models
{
    public class GenericClass<T>
    {
        public T Value { get; set; }
        public NestedInGeneric<T> OtherValue { get; set; }
    }

    public class NestedInGeneric<T>
    {
        public T InnerInner { get; set; }
    }
}
