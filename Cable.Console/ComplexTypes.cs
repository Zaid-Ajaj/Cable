using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
#if BRIDGE
    [Bridge.Reflectable]
#endif
    public enum Gender { Male, Female }

    #if BRIDGE
    [Bridge.Reflectable]
    #endif
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Money { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMarried { get; set; }
        public Gender Gender { get; set; }
    }

#if BRIDGE
    [Bridge.Reflectable]
#endif
    public class Generic<T>
    {
        public T Value { get; set; }
    }
}
