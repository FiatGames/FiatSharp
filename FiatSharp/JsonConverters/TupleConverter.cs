using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp.JsonConverters
{
    public class TupleConverter<T1,T2> : JsonConverter<Tuple<T1, T2>>
    {
        public override void WriteJson(JsonWriter writer, Tuple<T1, T2> value, JsonSerializer serializer)
        {
            JArray a = new JArray(JToken.FromObject(value.Item1, serializer), JToken.FromObject(value.Item2, serializer));
            a.WriteTo(writer);
        }

        public override Tuple<T1, T2> ReadJson(JsonReader reader, Type objectType, Tuple<T1, T2> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}