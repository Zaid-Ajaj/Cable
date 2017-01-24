using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Client
{
    public class Client
    {
        public static T Resolve<T>()
        {
            return default(T);
        }
    }
}
