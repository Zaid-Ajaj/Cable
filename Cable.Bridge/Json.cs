using Bridge;
using Bridge.Html5;
using System;

namespace Cable.Bridge
{
    public static class Json
    {
        public static string Serialize(object obj)
        {
            var encoded = Converters.EncodeObject(obj);
            var encodedWithoutBoxing = Converters.EliminateBoxing(encoded);
            return JSON.Stringify(encodedWithoutBoxing);
        }


        public static object SerializeToObjectLiteral(object obj, Type type)
        {
            var encoded = Converters.EncodeObject(obj, type);
            var encodedWithoutBoxing = Converters.EliminateBoxing(encoded);
            return encodedWithoutBoxing;
        }

        public static object Deserialize(string json, Type type)
        {
            var parsed = JSON.Parse(json);
            var decoded = Converters.DecodeObject(parsed, type);
            return decoded;
        }

        public static T Deserialize<T>(string json)
        {
            return Deserialize(json, typeof(T)).As<T>();
        }
    }
}
