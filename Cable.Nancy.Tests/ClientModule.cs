using Nancy;

namespace Cable.Nancy.Tests
{
    public class ClientModule : NancyModule
    {
        public ClientModule(IService service)
        {
            NancyServer.RegisterRoutesFor(this, service);
        }
    } 
}
