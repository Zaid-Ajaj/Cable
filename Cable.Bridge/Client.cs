﻿using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cable.Bridge
{
    public static class Client
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

        private static object PostJsonSync(string url, object[] data)
        {
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
                    var result = Json.Deserialize<object>(xmlHttp.ResponseText);
                    return result;
                }
            }
            else
            {
                throw new Exception("Error: " + xmlHttp.StatusText + "\n" + xmlHttp.ResponseText);
            }
        }

        static async Task<object> PostJsonAsync(string url, object[] data)
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

                        if (json["$exception"].As<bool>())
                        {
                            tcs.SetException(new Exception(json["$exceptionData"]["Message"].As<string>()));
                            return;
                        }
                        else
                        {
                            var result = Json.Deserialize<object>(xmlHttp.ResponseText);
                            tcs.SetResult(result);
                        }

                    }
                    else
                    {
                        tcs.SetException(new Exception(xmlHttp.StatusText));
                    }
                }

            };

            var serialized = Json.Serialize(data);
            xmlHttp.Send(serialized);
            return await tcs.Task;
        }

        public static T Resolve<T>()
        {
            var serviceType = typeof(T);

            var serviceName = serviceType.Name;
            var serviceFullName = serviceType.FullName.Replace(".", "$");

            if (InterfacesCache.ContainsKey(serviceFullName)) return InterfacesCache[serviceFullName].As<T>();

            if (!serviceType.IsInterface) throw new ArgumentException($"Type {serviceType.Name} must be an interface");

            var methods = serviceType.GetMethods();

            if (methods.Length == 0) throw new ArgumentException("The interface does not have any methods, if there are any methods then annotate the interface with the [Reflectable] attribute");

            var service = Script.Write<object>("{}");

            foreach (var method in methods)
            {
                var methodName = method.Name;
                if (method.ReturnType == typeof(Task))
                {
                    service[$"{serviceFullName}${methodName}"] = Lambda(async () =>
                    {
                        var parameters = Script.Write<object[]>("System.Linq.Enumerable.from(arguments).toArray()");
                        var url = $"/{serviceName}/{methodName}";
                        var result = await PostJsonAsync(url, parameters);
                        return result;
                    });
                }
                else
                {
                    service[$"{serviceFullName}${methodName}"] = Lambda(() =>
                    {
                        var parameters = Script.Write<object[]>("System.Linq.Enumerable.from(arguments).toArray()");
                        var url = $"/{serviceName}/{methodName}";
                        var result = PostJsonSync(url, parameters);
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