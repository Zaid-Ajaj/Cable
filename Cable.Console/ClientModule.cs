using Cable.Nancy;
using Client;
using Nancy;

namespace Cable.Console
{
    public class Home : NancyModule
    {
        public Home()
        {
            Get["/"] = _ => Response.AsFile("client/html/index.html");
            Get["js/{file}"] = p => Response.AsFile("client/js/" + (string)p.file);
            Get["html/{file}"] = p => Response.AsFile("client/html/" + (string)p.file);
            Get["css/{file}"] = p => Response.AsFile("client/css/" + (string)p.file);
        }
    }

    public class ClientModule : NancyModule
    {
        public ClientModule(IService service)
        {
            NancyServer.RegisterRoutesFor(this, service);
        }
    }
}
