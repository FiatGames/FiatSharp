using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp.JsonConverters
{
    public class TupleConverter<T1,T2> : JsonConverter<Tuple<T1, T2>>
    {
        public override void WriteJson(JsonWriter writer, Tuple<T1, T2> value, JsonSerializer serializer)
        {
            var v1 = value.Item1 == null
                ? null
                : JToken.FromObject(value.Item1, serializer);
            var v2 = value.Item2 == null
                ? null
                : JToken.FromObject(value.Item2, serializer);
            JArray a = new JArray(v1, v2);
            a.WriteTo(writer);
        }

        public override Tuple<T1, T2> ReadJson(JsonReader reader, Type objectType, Tuple<T1, T2> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}