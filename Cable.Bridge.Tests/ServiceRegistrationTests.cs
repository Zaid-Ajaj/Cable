using Bridge;
using Bridge.QUnit;
using System;
using System.Threading.Tasks;

namespace Cable.Bridge.Tests
{
    public static class ServiceRegistrationTests
    {
        interface INonReflectable
        {
            Task<int> Add(int x, int y);
        }

        interface IService
        {
            Task<int> Add(int x, int y);
        }

        public static void Run()
        {
            QUnit.Module("Service Registration Tests");

            //QUnit.Test("Cable.Resolve throws exception if interface not reflectable", assert => 
            //{
            //    try
            //    {
            //        var service = Client.Resolve<INonReflectable>();
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        assert.Equal(ex.Message, "The interface does not have any methods, if there are any methods then annotate the interface with the [Reflectable] attribute");
            //    }                
            //});

            //QUnit.Test("Cable.Resolve throws an exception if type is not an interface", assert => 
            //{
            //    try
            //    {
            //        var service = Client.Resolve<int>();
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        var msg = ex.Message;
            //        assert.Equal(msg, "Type Int32 must be an interface");
            //    }
            //});

            //QUnit.Test("Cable.Resolve throws exception if reflectable interface does not have methods", assert =>
            //{
            //    try
            //    {
            //        var service = Client.Resolve<IReflectable>();
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        var msg = ex.Message;
            //        assert.Equal(msg, "The interface does not have any methods, if there are any methods then annotate the interface with the [Reflectable] attribute");
            //    }
            //});

            QUnit.Test("Cable.Resolve doesn't throw exception on reflectable interface where all methods have return type of Task", assert =>
            {
                var service = BridgeClient.Resolve<IService>();
                assert.Equal(true, Script.IsDefined(service));
            });
        }
    }
}