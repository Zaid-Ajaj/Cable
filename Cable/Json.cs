using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable
{
    public static class Json
    {
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new CableConverter());
        }

        public static string Serialize(object obj, Formatting formatting)
        {
            return JsonConvert.SerializeObject(obj, formatting, new CableConverter());
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new CableConverter());
        }

        public static object Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, new CableConverter());
        } 

        public static T DefaultDeserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        
        public static string DefaultSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

    }
}
