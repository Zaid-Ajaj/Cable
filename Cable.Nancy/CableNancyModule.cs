using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Globalization;
using Cable;

namespace Cable.Nancy
{
    public class CableRoute
    {
        public string Method { get; set;  }
        public string Route { get; set; }
        public List<string> ParameterTypes { get; set; } = new List<string>();
        public string ReturnType { get; set; }
    }

    public class CableRouteSchema
    {
        public string ServiceInterface { get; set; }
        public string ServiceName { get; set; }
        public List<CableRoute> Routes { get; set; } = new List<CableRoute>();
    }


    public static class NancyServer
    {
        /// <summary>
        /// Customize how the route paths are generated. By default, the url mapper is (serviceName, methodName) => $"/{serviceName}/{methodName}".
        /// </summary>
        /// <param name="mapper"></param>
        public static void UrlMapper(Func<string, string, string> mapper)
        {
            if (mapper == null)
            {
                // Just keep using the default mapper
                return;
            }

            urlMapper = mapper;
        }

        private static Func<string, string, string> urlMapper = (serviceName, methodName) => $"/{serviceName}/{methodName}";

        private static bool CheckMethodOverloading<T>()
        {
            var type = typeof(T);

            if (!type.IsInterface) throw new ArgumentException("Injected service type must be an interface");

            var groups = typeof(T).GetMethods().Select(method => method.Name).GroupBy(x => x).Where(grouping => grouping.Count() > 1);

            if (groups.Count() == 0)
            {
                return false;
            }
            else
            {
                var methods = groups.Select(group => Tuple.Create(group.Key, group.Count()))
                                    .Select(tup => $"Method {tup.Item1} has {tup.Item2} overloads");

                var message = string.Join(", ", methods.ToArray());

                throw new Exception($"Overloading public methods in interface not supported: {message}.");
            }
                
        }


        private static string TypeName(Type type)
        {
            Func<Type, string> fullName = t => t.Name.Split('`')[0];

            if (!type.IsGenericType)
            {
                return $"{type.Name}";
            } 
            else
            {
                var genericArgs = type.GetGenericArguments();
                var asNames = string.Join(",", genericArgs.Select(TypeName));
                return $"{fullName(type)}<{asNames}>";
            }
        }


        public static bool EnableLogging = false;

        

        private static void Logger(string text)
        {
            if (!EnableLogging) return;

            var builder = new StringBuilder();
            builder.AppendLine("Cable:");
            builder.AppendLine(text);
            var output = builder.ToString();
            Console.WriteLine(output);
        }

        /// <summary>
        /// Generates POST routes automatically for the a NancyModule.
        /// </summary>
        /// <typeparam name="TBase">The type of the service (must be an interface)</typeparam>
        /// <param name="module">The NancyModule in which the routes will be created</param>
        /// <param name="implementation">An implementation of the service interface type</param>
        public static CableRouteSchema RegisterRoutesFor<TBase>(NancyModule module, TBase implementation)
        {
            var baseType = typeof(TBase);

            var type = implementation.GetType();

            var baseTypeName = TypeName(baseType);

            var serviceName = TypeName(type);

            var routeSchema = new CableRouteSchema
            {
                ServiceInterface = baseTypeName,
                ServiceName = serviceName
            };
            
            CheckMethodOverloading<TBase>();

            var defaultMethods = new string[] { "Equals", "ToString", "GetHashCode", "GetType" };

            var publicMethods = baseType.GetMethods().Where(method => !defaultMethods.Contains(method.Name));

            foreach(var method in publicMethods)
            {  
                if (method.ContainsGenericParameters) throw new ArgumentException($"Method {method.Name} contains generic type parameters, this is not supported.");
                RegisterRoute<TBase>(implementation, method, module, routeSchema);
            }
            return routeSchema;
        }


        static void RegisterRoute<TBase>(object implementation, MethodInfo method, NancyModule module, CableRouteSchema routeSchema)
        {
            var typeName = typeof(TBase).Name;
            var url = urlMapper(typeName, method.Name);
            var isReturningTask = method.ReturnType.BaseType == typeof(Task);

            routeSchema.Routes.Add(new CableRoute
            {
                Method = method.Name,
                ParameterTypes = method.GetParameters().Select(p => p.ParameterType).Select(TypeName).ToList(),
                ReturnType = TypeName(method.ReturnType),
                Route = url
            });

            module.Post[url, true] = async (ctx, token) =>
            {
                try
                {
                    using (var reader = new StreamReader(module.Request.Body))
                    {
                        var incomingJson = await reader.ReadToEndAsync();

                        var inputParameters = JObject.Parse(incomingJson);

                        var jsonParameters = inputParameters["Value"] as JArray;
                        var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

                        object[] parameters = new object[parameterTypes.Length];

                        for (var i = 0; i < parameters.Length; i++)
                        {
                            var type = parameterTypes[i];
                            // deserialize each parameter to it's respective type
                            var json = jsonParameters[i].ToString();
                            parameters[i] = Json.Deserialize(json, type);
                        }

                        object result = null;
                        if (isReturningTask)
                        {
                            dynamic task = method.Invoke(implementation, parameters);
                            result = await task;
                        }
                        else
                        {
                            result = method.Invoke(implementation, parameters);
                        }


                        return Json.Serialize(result);
                    }
                }
                catch (Exception ex)
                {
                    var errorData = new Dictionary<string, object>();
                    errorData["$exception"] = true;
                    errorData["$exceptionMessage"] = ex.InnerException.Message;
                    return JsonConvert.SerializeObject(errorData);
                }
            };
        }
    }
}
