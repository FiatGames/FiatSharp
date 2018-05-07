using System;
using System.Collections.Generic;
using FiatSharp.Models.Websocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp.JsonConverters
{
    public class ToServerCmdConverter<Settings, Move> : JsonConverter<ToServerCmd<Settings, Move>>
    {
        public override void WriteJson(JsonWriter writer, ToServerCmd<Settings, Move> value, JsonSerializer serializer)
        {
            serializer.Converters.Add(new FiatPlayerConverter());

            var a = new Dictionary<Type, Func<JObject>>
            {
                {
                    typeof(MakeMove<Settings,Move>), () => (JObject) JToken.FromObject(new {tag = "MakeMove", contents = ((MakeMove<Settings,Move>)value.Cmd).MoveToSubmit},serializer)
                },
                {
                    typeof(StartGame<Settings,Move>), () => (JObject) JToken.FromObject(new {tag = "StartGame"},serializer)
                },
                {
                    typeof(UpdateSettings<Settings,Move>), () => (JObject) JToken.FromObject(new {tag = "UpdateSettings", contents = ((UpdateSettings<Settings,Move>)value.Cmd).SettingsUpdate},serializer)
                }
            };
            JObject j = a[value.Cmd.GetType()]();
            j["player"] = JToken.FromObject(value.Player, serializer);
            j["hash"] = value.Hash.Hash;
            j.WriteTo(writer);
        }

        public override ToServerCmd<Settings, Move> ReadJson(JsonReader reader, Type objectType, ToServerCmd<Settings, Move> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}