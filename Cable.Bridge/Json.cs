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
            var stringified = JSON.Stringify(encoded);
            return stringified;
        }

        public static T Deserialize<T>(string json)
        {
            var parsed = JSON.Parse(json);
            var decoded = Converters.DecodeObject<T>(parsed).As<T>();
            return decoded;
        }
    }
}
