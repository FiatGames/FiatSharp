using System;
using FiatSharp.Models;
using FiatSharp.Models.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp.JsonConverters
{
    public class InitialGameStateResultConverter<Settings,State,Move> : JsonConverter<InitialGameStateResult<Settings, State, Move>>
    {
        public override void WriteJson(JsonWriter writer, InitialGameStateResult<Settings, State, Move> value, JsonSerializer serializer)
        {
            if (value.IsError)
            {
                JObject o = (JObject)JToken.FromObject(new { Left = value.Error });
                o.WriteTo(writer);
            }
            else
            {
                serializer.Converters.Add(new GameStateConverter<State, Move>());
                serializer.Converters.Add(new TupleConverter<Settings, GameState<State, Move>>());

                JObject o = (JObject)JToken.FromObject(new { Right = value.InitialGameState }, serializer);
                o.WriteTo(writer);
            }
        }
        public override InitialGameStateResult<Settings, State, Move> ReadJson(JsonReader reader, Type objectType, InitialGameStateResult<Settings, State, Move> existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}