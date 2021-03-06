﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Globalization;

namespace Cable
{
    public class CableConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        static bool IsEnumerableOfSomething(Type type)
        {
            return type.FullName.StartsWith("System.Collections.Generic.IEnumerable");
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            
            var json = JObject.Load(reader);

            var type = json["Type"].Value<string>();

            if (type == "NullType")
            {
                return null;
            }

            if (type == "DateTime")
            {
                var value = json["Value"].Value<string>();
                return DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
            else if (type == "Int64")
            {
                return long.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "UInt64")
            {
                return ulong.Parse(json["Value"].Value<string>());
            }
            else if (type == "Int32" && objectType != typeof(char))
            {
                return int.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "UInt32")
            {
                return uint.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "Int16")
            {
                return short.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "UInt16")
            {
                return ushort.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "Decimal")
            {
                return decimal.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "SByte")
            {
                return sbyte.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "Byte")
            {
                return byte.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "Single")
            {
                return float.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "Double")
            {
                return double.Parse(json["Value"].Value<string>(), CultureInfo.InvariantCulture);
            }
            else if (type == "Boolean")
            {
                return json["Value"].Value<bool>();
            }
            else if (type == "String")
            {
                return json["Value"].Value<string>();
            }
            else if (type == "Char" || objectType == typeof(char))
            {
                return (char)json["Value"].Value<int>();
            }
            else if (type == "Enum")
            {
                return Convert.ChangeType(Enum.Parse(objectType, json["Value"].Value<string>()), objectType);
            }
            else if (type == "TimeSpan")
            {
                return TimeSpan.FromTicks(long.Parse(json["Value"].Value<string>()));
            }
            else if (type == "Array")
            {
                var arr = (JArray)json["Value"];
                if (arr.Count > 0
                    && objectType != null
                    && objectType != typeof(object[])
                    && objectType != typeof(object)
                    && !IsEnumerableOfSomething(objectType)
                  )
                {
                    // get the type of the array
                    var arrType = objectType.GetElementType();
                    // create a typed array with the approriate length
                    var array = Array.CreateInstance(arrType, arr.Count);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        // deserialize each value of the array
                        var value = serializer.Deserialize(arr[i].CreateReader(), arrType);
                        array.SetValue(value, i);
                    }
                    return array;
                }
                else if (IsEnumerableOfSomething(objectType))
                {
                    var typeOfThatSomething = objectType.GetGenericArguments()[0];
                    var array = Array.CreateInstance(typeOfThatSomething, arr.Count);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        var value = serializer.Deserialize(arr[i].CreateReader(), typeOfThatSomething);
                        array.SetValue(value, i);
                    }
                    return array;
                }
                else if (objectType != null && arr.Count == 0)
                {
                    return Array.CreateInstance(objectType, 0);
                }
                else if (arr.Count == 0)
                {
                    return new object[] { };
                }
                else
                {
                    var array = new List<dynamic>();
                    for (int i = 0; i < arr.Count; i++)
                    {
                        // var value = serializer.Deserialize(arr[i].CreateReader());
                        var value = ReadJson(arr[i].CreateReader(), null, null, serializer);
                        array.Add((dynamic)value);
                    }
                    return array.ToArray();
                }

            }
            else if (type == "List")
            {
                var arr = (JArray)json["Value"];

                var listType = objectType.GetGenericArguments()[0];

                if (arr.Count > 0 && objectType != null)
                {

                    dynamic list = Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));
                    list.Capacity = arr.Count;
                    for (int i = 0; i < arr.Count; i++)
                    {
                        var value = serializer.Deserialize(arr[i].CreateReader(), listType);
                        list.Add((dynamic)value);
                    }
                    return list;
                }
                else
                {
                    return Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));
                }
            }
            else if (type == "Stack")
            {
                var arr = (JArray)json["Value"];

                var listType = objectType.GetGenericArguments()[0];
                dynamic stack = Activator.CreateInstance(typeof(Stack<>).MakeGenericType(listType));

                if (arr.Count > 0 && objectType != null)
                {

                    for (int i = arr.Count - 1; i >= 0; i--)
                    {
                        var value = serializer.Deserialize(arr[i].CreateReader(), listType);
                        stack.Push((dynamic)value);
                    }

                    return stack;
                }
                else
                {
                    return stack;
                }
            }
            else if (type == "Queue")
            {
                var arr = (JArray)json["Value"];

                var listType = objectType.GetGenericArguments()[0];
                dynamic queue = Activator.CreateInstance(typeof(Queue<>).MakeGenericType(listType));

                if (arr.Count > 0 && objectType != null)
                {

                    for (int i = 0; i < arr.Count; i++)
                    {
                        var value = serializer.Deserialize(arr[i].CreateReader(), listType);
                        queue.Enqueue((dynamic)value);
                    }

                    return queue;
                }
                else
                {
                    return queue;
                }
            }
            else
            {
                dynamic instance = null;
                
                try
                {
                    var typeName = json["Type"].Value<string>();
                    var objType = Type.GetType(typeName);
                    instance = Activator.CreateInstance(objType);
                }
                catch
                {
                    if (objectType != null)
                    {
                        instance = Activator.CreateInstance(objectType);
                    }
                    
                }
                finally
                {
                    if (instance == null)
                    {
                        instance = new object();
                    }

                }

                if (instance.GetType() == typeof(object))
                {
                    instance = new ExpandoObject();
                    var dict = instance as IDictionary<string, object>;
                    foreach (var prop in ((JObject)json["Value"]).Properties())
                    {
                        //var read = ReadJson(prop["Value"].CreateReader(), instance.GetType().GetProperty(prop.Name).GetType(), null, serializer);
                        var propObject = prop.Value;
                        var propValue = propObject["Value"];

                        dict[prop.Name] = ReadJson(prop.Value.CreateReader(), null, null, serializer);
                    }
                }
                else
                {
                    var objWithProps = new JObject();
                    foreach (var prop in ((JObject)json["Value"]).Properties())
                    {
                        //var read = ReadJson(prop["Value"].CreateReader(), instance.GetType().GetProperty(prop.Name).GetType(), null, serializer);
                        var propObject = prop.Value;
                        var propValue = propObject["Value"];
                        objWithProps.Add(new JProperty(prop.Name, prop.Value));
                    }
                    serializer.Populate(objWithProps.CreateReader(), instance);

                }

                return instance;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Converters.MakeJson(value).WriteTo(writer);
        }
    }
}
