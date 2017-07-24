using Nancy;

namespace Cable.Nancy.Tests
{
    public class ClientModule : NancyModule
    {
        public ClientModule(IService service)
        {
            var schema = NancyServer.RegisterRoutesFor(this, service);
        }
    } 
}
