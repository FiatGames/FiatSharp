using System;
using FiatSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp.JsonConverters
{
    public class GameStateConverter<S,M> : JsonConverter<GameState<S, M>>
    {
        public override void WriteJson(JsonWriter writer, GameState<S, M> value, JsonSerializer serializer)
        {
            serializer.Converters.Add(new GameStageConverter());
            serializer.Converters.Add(new FutureMoveConverter<M>());

            JObject o = (JObject)JToken.FromObject(new { });
            o.Add("_gameStateStage", JToken.FromObject(value.Stage, serializer));
            o.Add("_gameStateState", JToken.FromObject(value.State, serializer));
            if (value.FutureMove == null)
            {
                o.Add("_gameStateFutureMove", null);
            }
            else
            {
                o.Add("_gameStateFutureMove", JToken.FromObject(value.FutureMove, serializer));
            }
            

            o.WriteTo(writer);
        }

        public override GameState<S, M> ReadJson(JsonReader reader, Type objectType, GameState<S, M> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            JObject obj = JObject.Load(reader);
            serializer.Converters.Add(new GameStageConverter());
            serializer.Converters.Add(new FutureMoveConverter<M>());
            GameStage stage = obj["_gameStateStage"]
                .ToObject<GameStage>(serializer);

            S state = obj["_gameStateState"].ToObject<S>(serializer);


            FutureMove<M> futureMove = obj["_gameStateFutureMove"].Type == JTokenType.Null
                ? null
                : obj["_gameStateFutureMove"].ToObject<FutureMove<M>>(serializer);

            return new GameState<S, M> { State = state, FutureMove = futureMove, Stage = stage };
        }
    }
}