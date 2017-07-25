using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cable.Bridge
{
    public static class Converters
    {
        public static bool IsNumeric<T>(T value)
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

        public static bool IsPrimitive(object value)
        {
            return value is string
                || value is char
                || value is bool
                || value is DateTime
                || value is TimeSpan
                || IsNumeric(value); 
        }

        public static bool IsArray<T>(T obj)
        {
            var type = obj.GetType();

            var typeName = type.Name;
            var isBridgeGenerator = type.Name.StartsWith("GeneratorEnumerable");
            var fullNameEndsWithBrackets = type.FullName.EndsWith("[]");
            var hasAnToArrayProperty = Script.IsDefined(obj["toArray"]);

            return typeName == "Array"
                || isBridgeGenerator
                || fullNameEndsWithBrackets
                || typeName == "ArrayEnumerable"
                || Script.IsDefined(obj["getEnumerator"]);
        }



        public static Property[] ExtactExistingProps(object input)
        {
            var props = new List<Property>();
            var inputType = input.GetType();
            var properties = inputType.GetProperties();
            foreach(var prop in properties)
            {
                var property = new Property
                {
                    Name = prop.Name,
                    Type = prop.PropertyType,
                    Value = input[prop.Name]
                };

                props.Add(property);
            }

            return props.ToArray();
        }

        public static object EncodeDateTime(DateTime time)
        {
            var result = Script.Write<object>("{}");
            result["Type"] = "DateTime";
            result["Value"] = time.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            return EliminateBoxing(result);
        }

        public static object EncodeNumeric<T>(T value, string typeName = "")
        {
            var result = Script.Write<object>("{}");
            result["Type"] = string.IsNullOrEmpty(typeName) ? value.GetType().Name : typeName;
            result["Value"] = value.ToString();
            return EliminateBoxing(result);
        }

        public static object EncodeChar<T>(T value)
        {
            var result = Script.Write<object>("{}");
            result["Type"] = "Char";
            result["Value"] = value;
            return EliminateBoxing(result);
        }

        public static object EncodeString(object value)
        {
            var result = Script.Write<object>("{}");
            result["Type"] = "String";
            result["Value"] = value;
            return EliminateBoxing(result);
        }

        public static object EncodeBoolean(object value)
        {
            var result = Script.Write<object>("{}");
            result["Type"] = "Boolean";
            result["Value"] = value;
            return EliminateBoxing(result);
        }

        public static object EncodeTimeSpan(object timeSpan)
        {
            var result = Script.Write<object>("{}");
            result["Type"] = "TimeSpan";
            result["Value"] = timeSpan.As<TimeSpan>().Ticks.ToString();
            return EliminateBoxing(result);
        }

        [Template("Bridge.getType({value})")]
        static extern Type GetTypeof(object value);

        public static object EliminateBoxing(object encodedObject)
        {
            var keys = Object.Keys(encodedObject);
            for (var i = 0; i < keys.Length; i++)
            {
                var value = encodedObject[keys[i]];
                if (Script.IsDefined(value["$boxed"]))
                {
                    encodedObject[keys[i]] = value["v"];
                }
            }

            return encodedObject;
        }

        public static object EncodeObject(object value, string typeName = "")
        {
            var type = value.GetType();

            if (IsPrimitive(value))
            {
                if (type == typeof(DateTime))
                {
                    return EncodeDateTime(value.As<DateTime>());
                }
                else if (type == typeof(char))
                {
                    return EncodeChar(value);
                }
                else if (IsNumeric(value))
                {
                    return EncodeNumeric(value, typeName);
                }
                else if (type == typeof(string))
                {
                    return EncodeString(value);
                }
                
                else if (type == typeof(bool))
                {
                    return EncodeBoolean(value);
                }

                else if (type == typeof(TimeSpan))
                {
                    return EncodeTimeSpan(value);
                }

                return null;


            }
            else if (IsArray(value))
            {
                var result = Script.Write<object>("{}");

                if (type.Name == "Array")
                {
                    result["Type"] = "Array";
                    var values = Script.Write<object[]>("[]");
                    for(int i = 0; i < Script.Write<int>("value.length"); i++)
                    {
                        var subValue = EncodeObject(Script.Write<object>("value[i]"));
                        subValue = EliminateBoxing(subValue);
                        Script.Write("values.push(subValue)");
                    }

                    result["Value"] = values;
                    return result;
                }
                else if (type.FullName.StartsWith("System.Collections.Generic.List"))
                {
                    result["Type"] = "List";
                    var list = Script.Write<object[]>("value.toArray()");
                    var valuesFromList = new object[list.Length];

                    for (int i = 0; i < list.Length; i++)
                    {
                        var encodedObject = EncodeObject(list[i]);
                        valuesFromList[i] = EliminateBoxing(encodedObject);
                    }

                    result["Value"] = valuesFromList;
                    return result;
                }
                else if (type.Name == "ArrayEnumerable")
                {
                    result["Type"] = "Array";
                    var list = Script.Write<object[]>("System.Linq.Enumerable.from(value).toArray()");
                    var valuesFromList = new object[list.Length];

                    for (int i = 0; i < list.Length; i++)
                    {
                        var encodedObject = EncodeObject(list[i]);
                        valuesFromList[i] = EliminateBoxing(encodedObject);
                    }

                    result["Value"] = valuesFromList;
                    return result;
                }
                else
                {
                    try
                    {
                        result["Type"] = "Array";
                        var list = Script.Write<object[]>("System.Linq.Enumerable.from(value).toArray()");
                        var valuesFromList = new object[list.Length];

                        for (int i = 0; i < list.Length; i++)
                        {
                            var encodedObject = EncodeObject(list[i]);
                            valuesFromList[i] = EliminateBoxing(encodedObject);
                        }

                        result["Value"] = valuesFromList;
                        return result;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            else
            {
                // then it is an object here
                var result = new object();
                result["FromBridge"] = true;
                result["Type"] = GetTypeof(value).FullName;

                var props = new object();

                foreach(var prop in ExtactExistingProps(value))
                {
                    var nextValue = EncodeObject(prop.Value, prop.Type.Name);
                    props[prop.Name] = EliminateBoxing(nextValue);
                }

                result["Value"] = props;
               
                return result;
            }
        }

        static double ParseDouble(string input)
        {
            return Script.Write<double>("parseFloat(input, 10)");
        }

        public static object DecodeObject(object json, Type jsonType)
        {
            var type = json["Type"].As<string>();

            if (type == "NullType")
            {
                return null;
            }
            else if (type == "DateTime")
            {
                var format = "dd/MM/yyyy HH:mm:ss.fff";
                var result = DateTime.ParseExact(json["Value"].As<string>(), format, CultureInfo.InvariantCulture);
                return EliminateBoxing(result as object);
            }
            else if (type == "String") return json["Value"];

            else if(type == "Char") return (char)(json["Value"].As<int>());

            else if(type == "TimeSpan") return TimeSpan.FromTicks(long.Parse(json["Value"].As<string>()));

            else if(type == "UInt16") return ushort.Parse(json["Value"].As<string>());

            else if(type == "UInt32") return uint.Parse(json["Value"].As<string>());

            else if(type == "UInt64") return ulong.Parse(json["Value"].As<string>());

            else if(type == "Int16") return short.Parse(json["Value"].As<string>());

            else if(type == "Int32") return int.Parse(json["Value"].As<string>());

            else if(type == "Int64") return long.Parse(json["Value"].As<string>());

            else if(type == "Double") return ParseDouble(json["Value"].As<string>());

            else if(type == "Decimal") return decimal.Parse(json["Value"].As<string>());

            else if(type == "Byte") return byte.Parse(json["Value"].As<string>());

            else if(type == "Boolean") return json["Value"].As<bool>();

            else if(type == "Enum")
            {
                var enumType = Type.GetType(json["EnumType"].As<string>());
                var enumValue = Enum.Parse(enumType, json["Value"].As<string>());
                return enumValue;
            }
            else if (type  == "Array")
            {
                var encodedItems = json["Value"].As<object[]>();
                var arr = new object[encodedItems.Length];

                for (int i = 0; i < encodedItems.Length; i++)
                {
                    arr[i] = DecodeObject(encodedItems[i], typeof(object));
                }

                return arr;
            }
            else if (type == "List")
            {
                var encodedItems = json["Value"].As<object[]>();
                var list = new List<object>();
                for (int i = 0; i < encodedItems.Length; i++)
                {
                    list.Add(DecodeObject(encodedItems[i], typeof(object)));
                }

                return list;
            }
            else if (Script.IsDefined(json["FromBridge"]))
            {
                object instance = null;

                if (json["FromBridge"].As<bool>())
                {
                    var typeFullName = json["Type"].As<string>();
                    var typeFromAssembly = Type.GetType(typeFullName);
                    instance = Activator.CreateInstance(typeFromAssembly);
                }
                else
                {
                    var typeGenerationExpression = json["Type"].As<string>();
                    instance = Script.Eval<object>(typeGenerationExpression);
                }


                var propNames = ObjectKeys(json["Value"]);
                for (int i = 0; i < propNames.Length; i++)
                {
                    var propValue = json["Value"][propNames[i]];
                    var decoded = DecodeObject(propValue, typeof(object));
                    var setPropName = propNames[i];
                    instance[setPropName] = decoded;
                }

                return instance;
            }
            else
            {
                return null;
            }
        }

        static string[] ObjectKeys(object obj)
        {
            return Script.Write<string[]>("Object.keys(obj)");
        }
    } 
}