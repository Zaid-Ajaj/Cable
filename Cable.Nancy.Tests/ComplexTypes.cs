using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Nancy.Tests
{
    public enum Gender { Male, Female }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Money { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMarried { get; set; }
        public Gender Gender { get; set; }
    }


    public class Generic<T>
    {
        public T Value { get; set; }
    }
}
