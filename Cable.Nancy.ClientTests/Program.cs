using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Bridge.QUnit;

namespace Cable.Nancy.ClientTests
{
    public class Program
    {
        public static void Main()
        {
            QUnit.Test("Tests are working", assert =>
            {
                assert.Equal(true, true);
            });
        }
    }
}
