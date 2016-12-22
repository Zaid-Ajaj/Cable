using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Nancy.Hosting.Self;

namespace Cable.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:8080")))
            {
                System.Console.WriteLine("Running Nancy server on http://localhost:8080");
                host.Start();
                System.Console.ReadLine();
            }
        }
    }

}
