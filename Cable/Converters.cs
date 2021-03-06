﻿using Newtonsoft.Json.Linq;
using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;

namespace Cable
{
    public static class Converters
    {
        public static bool IsNumber(object value)
        {
            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }

        public static JObject Enum(object enumObject)
        {
            var json = new JObject();
            json.Add(new JProperty("Type", "Enum"));
            json.Add(new JProperty("EnumType", enumObject.GetType().FullName));
            json.Add(new JProperty("Value", enumObject.ToString()));

            return json;
        }

        public static JObject DateTime(DateTime time)
        {
            var obj = new JObject();
            obj.Add(new JProperty("Type", "DateTime"));
            obj.Add(new JProperty("Value", time.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture)));
            return obj;
        }

        public static JObject String(string input)
        {
            var strJson = new JObject();
            strJson.Add(new JProperty("Type", "String"));
            strJson.Add(new JProperty("Value", input));
            return strJson;
        }

        public static JObject Char(char token)
        {
            var charJson = new JObject();
            charJson.Add(new JProperty("Type", "Char"));
            charJson.Add(new JProperty("Value", (int)token));
            return charJson;
        }

        public static JObject NullValue()
        {
            var json = new JObject();
            json.Add(new JProperty("Type", "NullType"));
            json.Add(new JProperty("Value", null));
            return json;
        }
        public static JObject Numeric(object value)
        {
            if (!IsNumber(value))
            {
                throw new ArgumentException("value must have a numeric type");
            }

            var json = new JObject();
            json.Add(new JProperty("Type", value.GetType().Name));

            if (value.GetType() == typeof(double))
            {
                json.Add(new JProperty("Value", ((double)value).ToString(CultureInfo.InvariantCulture)));
            }
            else if (value.GetType() == typeof(int))
            {
                json.Add(new JProperty("Value", ((int)value).ToString(CultureInfo.InvariantCulture)));
            }
            else if (value.GetType() == typeof(decimal))
            {
                json.Add(new JProperty("Value", ((decimal)value).ToString(CultureInfo.InvariantCulture)));
            }            
            else if (value.GetType() == typeof(long))
            {
                json.Add(new JProperty("Value", ((long)value).ToString(CultureInfo.InvariantCulture)));
            }
            else if (value.GetType() == typeof(float))
            {
                json.Add(new JProperty("Value", ((float)value).ToString(CultureInfo.InvariantCulture)));
            }
            else if (value.GetType() == typeof(byte))
            {
                json.Add(new JProperty("Value", ((byte)value).ToString(CultureInfo.InvariantCulture)));
            }
            else if (value.GetType() == typeof(uint))
            {
                json.Add(new JProperty("Value", ((uint)value).ToString(CultureInfo.InvariantCulture)));
            }
            else if (value.GetType() == typeof(ulong))
            {
                json.Add(new JProperty("Value", ((ulong)value).ToString(CultureInfo.InvariantCulture)));
            }
            else if (value.GetType() == typeof(sbyte))
            {
                json.Add(new JProperty("Value", ((sbyte)value).ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                json.Add(new JProperty("Value", value.ToString()));
            }
            return json;
        }

        public static JObject Boolean(bool value)
        {
            var json = new JObject();
            json.Add(new JProperty("Type", "Boolean"));
            json.Add(new JProperty("Value", value));

            return json;
        }

        public static JObject TimeSpan(TimeSpan span)
        {
            var json = new JObject();
            json.Add(new JProperty("Type", "TimeSpan"));
            json.Add(new JProperty("Value", span.Ticks.ToString()));

            return json;
        }

        public static object GetPropertyValue(object obj, string prop)
        {
            return obj.GetType().GetProperty(prop).GetValue(obj);
        }

        private static string CollectionType(Type type)
        {
            if (type.FullName.EndsWith("[]")) return "Array";
            else if (type.FullName.StartsWith("System.Collections.Generic.List") 
                  || type.FullName.StartsWith("System.Collections.Generic.IList"))
            {
                return "List";
            }
            else if (type.FullName.StartsWith("System.Collections.Generic.Queue"))
            {
                return "Queue";
            }
            else if (type.FullName.StartsWith("System.Collections.Generic.Stack"))
            {
                return "Stack";
            }
            else if (type.FullName.StartsWith("System.Collections.Generic.IEnumerable"))
            {
                return "Array";
            }
            else if (type.FullName.StartsWith("System.Linq.Enumerable"))
            {
                return "Array";
            }

            return "Unknown";

        }
        public static JObject MakeJson(object obj)
        {
            if (obj == null)
            {
                return NullValue();
            }

            JToken token = JToken.FromObject(obj);
            if (token.Type != JTokenType.Object && token.Type != JTokenType.Array)
            {
                // then it must be primitive
                var type = obj.GetType();

                if (type == typeof(DateTime))
                {
                    var dateTime = (DateTime)obj;
                    return DateTime(dateTime);
                }
                else if (IsNumber(obj))
                {
                    return Numeric(obj);
                }
                else if (type == typeof(string))
                {
                    return String((string)obj);
                }
                else if (type == typeof(char))
                {
                    return Char((char)obj);
                }
                else if (type == typeof(bool))
                {
                    return Boolean((bool)obj);
                }
                else if (type.IsEnum)
                {
                    return Enum(obj);
                }
                else if (type == typeof(TimeSpan))
                {
                    return TimeSpan((TimeSpan)obj);
                }
                else
                {
                    return ((JObject)token);
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                var arrJson = new JObject();
                var arr = new JArray();

                foreach(var item in (dynamic)obj)
                {
                    arr.Add(MakeJson(item));
                }
                
                arrJson.Add(new JProperty("Type", CollectionType(obj.GetType())));
                arrJson.Add(new JProperty("Value", arr));
                return arrJson;
            }
            else
            {
                var composite = (JObject)token;
                var clone = new JObject();
                clone.Add(new JProperty("FromBridge", false));
                // the Type property is only meant for Bridge client apps
                // this will be a string that when evaluated in javascript
                // will behave the same as making a new instance of the (generic) class
                // i.e. eval(typeName(class)) = Activator.CreateInstance(class)
                clone.Add(new JProperty("Type", TypeName(obj.GetType())));

                var value = new JObject();
                foreach(var prop in composite.Properties())
                {
                    var propName = prop.Name;
                    value.Add(new JProperty(propName, MakeJson(GetPropertyValue(obj, propName))));
                }

                clone.Add(new JProperty("Value", value));
                return clone;
            }
        }


        /// <summary>
        /// Generates a string output from a type such that eval(output) = Activator.CreateInstance(type)
        /// </summary>
        private static string TypeName(Type type)
        {
            Func<Type, string> fullName = t => t.FullName.Split('`')[0];

            if (!type.IsGenericType && !type.IsPrimitive)
            {
                return $"new {type.FullName}()";
            }
            else if (type.IsPrimitive)
            {
                return $"{type.FullName}";
            }
            else
            {
                var genericArgs = type.GetGenericArguments();
                var asNames = string.Join(",", genericArgs.Select(TypeName));
                return $"new ({fullName(type)}${genericArgs.Length}({asNames}))()";
            }
        }
    }
}
