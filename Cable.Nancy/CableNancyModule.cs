using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

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
            
            

            if (!baseType.IsInterface) throw new ArgumentException("Generic type argument TBase must be an interface");

            var defaultMethods = new string[] { "Equals", "ToString", "GetHashCode", "GetType" };

            foreach(var method in type.GetMethods())
            {
                if (defaultMethods.Contains(method.Name)) continue;
               
                RegisterRoute<TBase>(implementation, method, module, routeSchema);
            }

            return routeSchema;
        }

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

        static void RegisterRoute<TBase>(object implementation, MethodInfo method, NancyModule module, CableRouteSchema routeSchema)
        {
            var typeName = typeof(TBase).Name;
            var url = $"/{typeName}/{method.Name}";
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

                        dynamic data;

                        dynamic[] paramArrayValue;

                        dynamic paramArray = Json.DefaultDeserialize<dynamic>(incomingJson);

                        paramArrayValue = new dynamic[paramArray.Value.Count];

                        for (var i = 0; i < paramArrayValue.Length; i++)
                        {
                            paramArrayValue[i] = Json.DefaultSerialize(paramArray.Value[i]);
                        }

                        var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

                        object[] parameters = new object[paramArrayValue.Length];

                        for (var i = 0; i < parameters.Length; i++)
                        {
                            var type = parameterTypes[i];

                            var genericCableDeserializer = typeof(Json).GetMethod("Deserialize").MakeGenericMethod(type);

                            var deserialized = genericCableDeserializer.Invoke(null, new object[] { paramArrayValue[i] });

                            if (deserialized.GetType() == typeof(ExpandoObject))
                            {
                                var genericDeserializer = typeof(Json).GetMethod("DefaultDeserialize").MakeGenericMethod(type);
                                var serialized = Json.DefaultSerialize(deserialized as IDictionary<string, object>);
                                parameters[i] = genericDeserializer.Invoke(null, new object[] { serialized });
                            }
                            else if (type == typeof(char))
                            {
                                parameters[i] = (char)deserialized;
                            }
                            else 
                            {
                                parameters[i] = deserialized;
                            }
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
                    errorData["$exceptionData"] = ex;
                    return Json.DefaultSerialize(errorData);
                }
            };
        }

    }
}
