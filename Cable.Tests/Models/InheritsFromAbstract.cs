using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Tests.Models
{
    public abstract class Abstract
    {
        public int Id { get; set; }
    }

    public class InheritsFromAbstract : Abstract
    {
        public string Value { get; set; }
    }
}
