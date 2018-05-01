using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp
{
    public class FiatPlayer
    {
        public bool IsSystem => Id == null;
        public int? Id { get; set; }
    }

    public class FiatPlayerConverter : JsonConverter<FiatPlayer>
    {
        public override void WriteJson(JsonWriter writer, FiatPlayer value, JsonSerializer serializer)
        {
            JObject t = value.IsSystem
                ? (JObject)JToken.FromObject(new { tag = "System" })
                : (JObject)JToken.FromObject(new { tag = "FiatPlayer", contents = value.Id });

            t.WriteTo(writer);
        }

        public override FiatPlayer ReadJson(JsonReader reader, Type objectType, FiatPlayer existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            switch (obj["tag"].ToObject<string>())
            {
                case "System":
                    return new FiatPlayer { Id = null };
                case "FiatPlayer":
                    return new FiatPlayer { Id = obj["contents"].ToObject<int>() };
            }
            throw new Exception("Not valid FiatPlayer JSON");
        }

    }
}