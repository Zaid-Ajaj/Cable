using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace Cable.Bridge
{
    public static class BridgeClient
    {
        private static Dictionary<string, object> InterfacesCache = new Dictionary<string, object>();

        static string ToCamelCase(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var output = "";
                output += char.ToLower(input[0]).ToString();

                for (int i = 1; i < input.Length; i++)
                {
                    output += input[i];
                }

                return output;
            }

            return "";
        }

        private static object PostJsonSync(string url, Type returnType, object[] data)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var xmlHttp = new XMLHttpRequest();
            xmlHttp.Open("POST", url, false);

            var serialized = Json.Serialize(data);

            xmlHttp.Send(serialized);

            if (xmlHttp.Status == 200 || xmlHttp.Status == 304)
            {
                var json = JSON.Parse(xmlHttp.ResponseText);

                if (json["$exception"].As<bool>())
                {
                    throw new Exception(json["$exceptionData"]["Message"].As<string>());
                }
                else
                {
                    var result = Json.Deserialize(xmlHttp.ResponseText, returnType);
                    return result;
                }
            }
            else
            {
                throw new Exception("Error: " + xmlHttp.StatusText + "\n" + xmlHttp.ResponseText);
            }
        }

        static async Task<object> PostJsonAsync(string url, Type returnType, object[] parameters)
        {
            var tcs = new TaskCompletionSource<object>();
            var xmlHttp = new XMLHttpRequest();
            xmlHttp.Open("POST", url, true);
            xmlHttp.SetRequestHeader("Content-Type", "application/json");
            xmlHttp.OnReadyStateChange = () =>
            {
                if (xmlHttp.ReadyState == AjaxReadyState.Done)
                {
                    if (xmlHttp.Status == 200 || xmlHttp.Status == 304)
                    {

                        var json = JSON.Parse(xmlHttp.ResponseText);

                        if (json == null)
                        {
                            tcs.SetResult(null);
                        }
                        else if (Script.IsDefined(json["$exception"]) && json["$exception"].As<bool>())
                        {
                            tcs.SetException(new Exception(json["$exceptionData"]["Message"].As<string>()));
                        }
                        else
                        {
                            var result = Json.Deserialize(xmlHttp.ResponseText, returnType);
                            tcs.SetResult(result);
                        }
                    }
                    else
                    {
                        tcs.SetException(new Exception(xmlHttp.ResponseText));
                    }
                }

            };

            var serialized = Json.Serialize(parameters);
            xmlHttp.Send(serialized);
            return await tcs.Task;
        }

        static string Capitalized(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return char.ToUpper(input[0]).ToString() + input.Substring(1, input.Length - 1);
        }

        public static Func<string, string, string> UrlMapper = (serviceName, methodName) => $"/{serviceName}/{methodName}";

        public static Action<string> Logger = x => Script.Call("console.log", x);

        private static bool IsTask(Type t)
        {
            return t.FullName == "System.Threading.Tasks.Task`1";
        }

        public static T Resolve<T>()
        {
            
            var serviceType = typeof(T);

            var serviceName = serviceType.Name;
            var serviceFullName = serviceType.FullName.Replace(".", "$");

            if (InterfacesCache.ContainsKey(serviceFullName)) return InterfacesCache[serviceFullName].As<T>();

            if (!serviceType.IsInterface) throw new ArgumentException($"Type {serviceType.Name} must be an interface");

            var methods = serviceType.GetMethods();

            if (methods.Length == 0) throw new ArgumentException("The interface does not have any methods");

            // create an object literal that operates as a typed-proxy 
            var service = Script.Write<object>("{}");

            foreach (var method in methods)
            {
                var methodName = method.Name;
                var instanceMethodName = $"{serviceFullName}${Capitalized(methodName)}";
                if (IsTask(method.ReturnType))
                {
                    var taskArgs = method.ReturnType.GetGenericArguments();
                    // var taskType = taskArgs[0];
                    service[instanceMethodName] = Lambda(async () =>
                    {
                        var parameters = Script.Write<object[]>("System.Linq.Enumerable.from(arguments).toArray()");
                        var url = UrlMapper(serviceName, Capitalized(methodName));
                        var result = await PostJsonAsync(url, typeof(object), parameters);
                        return result;
                    });
                }
                else
                {
                    service[instanceMethodName] = Lambda(() =>
                    {
                        var parameters = Script.Write<object[]>("System.Linq.Enumerable.from(arguments).toArray()");
                        var url = UrlMapper(serviceName, Capitalized(methodName));
                        var result = PostJsonSync(url, method.ReturnType, parameters);
                        return result;
                    });
                }
            }

            InterfacesCache.Add(serviceFullName, service);

            return service.As<T>();
        }


        [Template("{0}")]
        extern static Func<T> Lambda<T>(Func<T> x);
    }
}