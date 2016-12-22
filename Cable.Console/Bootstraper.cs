using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.TinyIoc;
using Client;

namespace Cable.Console
{
    public class Bootstraper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            container.Register<IService, Service>();
        }
    }
}
