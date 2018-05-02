using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp.JsonConverters
{
    public class FutureMoveConverter<T> : JsonConverter<FutureMove<T>>
    {
        private static readonly JsonSerializer DateTimeJsonSerializer =
            new JsonSerializer {DateTimeZoneHandling = DateTimeZoneHandling.Utc};

        public override void WriteJson(JsonWriter writer, FutureMove<T> value, JsonSerializer serializer)
        {
            JObject o = (JObject)JToken.FromObject(new { });
            o.Add("_futureMoveMove", JToken.FromObject(value.Move, serializer));
            o.Add("_futureMoveTime", JToken.FromObject(value.DateTime.ToUniversalTime(), DateTimeJsonSerializer));
            o.WriteTo(writer);
        }

        public override FutureMove<T> ReadJson(JsonReader reader, Type objectType, FutureMove<T> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            JObject obj = JObject.Load(reader);
            T move = obj["_futureMoveMove"].ToObject<T>(serializer);
            DateTime dt = obj["_futureMoveTime"].ToObject<DateTime>(DateTimeJsonSerializer).ToLocalTime();
            return new FutureMove<T>
            {
                Move = move,
                DateTime = dt
            };
        }
    }
}