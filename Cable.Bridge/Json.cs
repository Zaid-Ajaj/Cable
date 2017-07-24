using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static object Deserialize(string json, Type type)
        {
            var parsed = JSON.Parse(json);
            var decoded = Converters.DecodeObject(parsed, type);
            return decoded;
        }
    }
}
