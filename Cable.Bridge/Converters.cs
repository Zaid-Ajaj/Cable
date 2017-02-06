using Bridge;
using System;
using System.Collections.Generic;

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
            var type = GetTypeof(obj);

            return type.Name == "Array"
                || type.FullName.EndsWith("[]")
                || type.Name == "ArrayEnumerable"
                || Script.IsDefined(obj["toArray"]);
        }



        public static Property[] ExtactExistingProps(object input)
        {
            var keys = Object.Keys(input);
            var props = Script.Write<Property[]>("[]");

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].StartsWith("$"))
                    continue;

                var property = new Property
                {
                    Name = keys[i],
                    Type = GetTypeof(input[keys[i]]),
                    Value = input[keys[i]]
                };


                Script.Write("props.push(property)");
            }

            return props;
        }

        public static object EncodeDateTime(DateTime time)
        {
            var result = Script.Write<object>("{}");
            result["IsPrimitive"] = true;
            result["IsArray"] = false;
            result["Type"] = "DateTime";
            result["IsNumeric"] = false;

            var value = Script.Write<object>("{}");
            value["Year"] = time.Year;
            value["Month"] = time.Month;
            value["Day"] = time.Day;
            value["Hour"] = time.Hour;
            value["Minute"] = time.Minute;
            value["Second"] = time.Second;
            value["Millisecond"] = time.Millisecond;

            result["Value"] = value;
            return result;
        }

        public static object EncodeNumeric<T>(T value)
        {
            var result = Script.Write<object>("{}");
            result["IsPrimitive"] = true;
            result["IsArray"] = false;
            result["IsNumeric"] = true;
            result["Type"] = GetTypeof(value).Name;
            result["Value"] = value.ToString();
            return result;
        }

        public static object EncodeChar<T>(T value)
        {
            var result = Script.Write<object>("{}");
            result["IsPrimitive"] = true;
            result["IsArray"] = false;
            result["IsNumeric"] = false;
            result["Type"] = "Char";
            result["Value"] = value;
            return result;
        }

        public static object EncodeString(object value)
        {
            var result = Script.Write<object>("{}");
            result["IsPrimitive"] = true;
            result["IsArray"] = false;
            result["IsNumeric"] = false;
            result["Type"] = "String";
            result["Value"] = value;
            return result;
        }

        public static object EncodeBoolean(object value)
        {
            var result = Script.Write<object>("{}");
            result["IsPrimitive"] = true;
            result["IsArray"] = false;
            result["IsNumeric"] = false;
            result["Type"] = "Boolean";
            result["Value"] = value;
            return result;
        }

        public static object EncodeTimeSpan(object timeSpan)
        {
            var result = Script.Write<object>("{}");
            result["IsPrimitive"] = true;
            result["IsArray"] = false;
            result["IsNumeric"] = false;
            result["Type"] = "TimeSpan";
            result["Value"] = timeSpan.As<TimeSpan>().Ticks.ToString();
            return result;
        }

        [Template("Bridge.getType({value})")]
        static extern Type GetTypeof(object value);

        public static object EncodeObject(object value)
        {
            var type = GetTypeof(value);

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
                    return EncodeNumeric(value);
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
                result["IsPrimitive"] = true;
                result["IsArray"] = true;
                result["IsNumeric"] = false;
                
                if (type.Name == "Array")
                {
                    result["Type"] = "Array";
                    var values = Script.Write<object[]>("[]");
                    for(int i = 0; i < Script.Write<int>("value.length"); i++)
                    {
                        var subValue = EncodeObject(Script.Write<object>("value[i]"));
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
                        valuesFromList[i] = EncodeObject(list[i]);
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
                        valuesFromList[i] = EncodeObject(list[i]);
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
                            valuesFromList[i] = EncodeObject(list[i]);
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
                result["IsPrimitive"] = false;
                result["IsArray"] = false;
                result["IsNumeric"] = false;
                result["FromBridge"] = true;
                result["Type"] = GetTypeof(value).FullName;

                var props = new object();

                foreach(var prop in ExtactExistingProps(value))
                {

                    //@ props[prop.getName()] = Cable.Bridge.Converters.encodeObject(prop.getValue());
                }

                result["Value"] = props;
               
                return result;
            }
        }

        public static object DecodeObject<T>(object json)
        {
            var type = json["Type"].As<string>();

            if (type == "NullType")
            {
                return null;
            }

            if (json["IsPrimitive"].As<bool>() && !json["IsArray"].As<bool>())
            {
                if (type == "DateTime")
                {
                    var year = json["Value"]["Year"].As<int>();
                    var month = json["Value"]["Month"].As<int>();
                    var day = json["Value"]["Day"].As<int>();
                    var hour = json["Value"]["Hour"].As<int>();
                    var minute = json["Value"]["Minute"].As<int>();
                    var second = json["Value"]["Second"].As<int>();
                    var milliseconds = json["Value"]["Millisecond"].As<int>();
                    return new DateTime(year, month, day, hour, minute, second, milliseconds);
                }

                if (type == "String") return json["Value"];

                if (type == "Char") return (char)(json["Value"].As<int>());

                if (type == "TimeSpan") return TimeSpan.FromTicks(long.Parse(json["Value"].As<string>()));

                if (type == "UInt16") return ushort.Parse(json["Value"].As<string>());

                if (type == "UInt32") return uint.Parse(json["Value"].As<string>());

                if (type == "UInt64") return ulong.Parse(json["Value"].As<string>());

                if (type == "Int16") return short.Parse(json["Value"].As<string>());

                if (type == "Int32") return int.Parse(json["Value"].As<string>());

                if (type == "Int64") return long.Parse(json["Value"].As<string>());

                if (type == "Double") return double.Parse(json["Value"].As<string>());

                if (type == "Decimal") return decimal.Parse(json["Value"].As<string>());

                if (type == "Byte") return byte.Parse(json["Value"].As<string>());

                if (type == "Boolean") return json["Value"].As<bool>();

                if (type == "Enum")
                {
                    var enumType = Type.GetType(json["EnumType"].As<string>());
                    var enumValue = Enum.Parse(enumType, json["Value"].As<string>());
                }

                return null;

            }
            else if (json["IsArray"].As<bool>())
            {
                if (json["Type"].As<string>() == "Array")
                {
                    
                    var encodedItems = json["Value"].As<object[]>();
                    var arr = new object[encodedItems.Length];

                    for (int i = 0; i < encodedItems.Length; i++)
                    {
                        arr[i] = DecodeObject<object>(encodedItems[i]);
                    }

                    return arr;
                }

                if (json["Type"].As<string>() == "List")
                {
                    var encodedItems = json["Value"].As<object[]>();
                    var list = new List<object>();

                    for (int i = 0; i < encodedItems.Length; i++)
                    {
                        list.Add(DecodeObject<object>(encodedItems[i]));
                    }

                    return list;
                }

                return null;
            }
            else
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
                    var decoded = DecodeObject<object>(propValue);
                    var setPropName = "set" + propNames[i];
                    instance[setPropName].As<Action<object>>()(decoded);
                }

                return instance;
            }
        }

        static string[] ObjectKeys(object obj)
        {
            return Script.Write<string[]>("Object.keys(obj)");
        }
    } 
}