using Cable.Nancy;
using Client;
using Nancy;

namespace Cable.Console
{
    public class ClientModule : NancyModule
    {
        public ClientModule(IService service)
        {
            var schema = NancyServer.RegisterRoutesFor(this, service);
        }
    }
}
