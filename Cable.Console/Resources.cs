using Nancy;

namespace Cable.Console
{
    public class Resources : NancyModule
    {
        public Resources()
        {
            Get["/"] = _ => Response.AsFile("client/html/index.html");
            Get["js/{file}"] = p => Response.AsFile("client/js/" + (string)p.file);
            Get["html/{file}"] = p => Response.AsFile("client/html/" + (string)p.file);
            Get["css/{file}"] = p => Response.AsFile("client/css/" + (string)p.file);
        }
    }
}
