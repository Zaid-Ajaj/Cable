using Nancy;

namespace Cable.Console
{
    public class Resources : NancyModule
    {
        public Resources()
        {
            Get["/"] = _ => Response.AsFile("client/index.html");
            Get["js/{file}"] = p => Response.AsFile("client/js/" + (string)p.file);
        }
    }
}
