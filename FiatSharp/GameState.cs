using System;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace FiatSharp
{
    public class GameState<S, M>
    {
        public GameStage Stage { get; set; }
        public S State { get; set; }
        public FutureMove<M> FutureMove { get; set; }
    }

    public class GameStateConverter<S,M> : JsonConverter<GameState<S, M>>
    {
        public override void WriteJson(JsonWriter writer, GameState<S, M> value, JsonSerializer serializer)
        {
            serializer.Converters.Add(new GameStageConverter());
            serializer.Converters.Add(new FutureMoveConverter<M>());

            JObject o = (JObject)JToken.FromObject(new { });
            o.Add("_gameStateStage", JToken.FromObject(value.Stage, serializer));
            o.Add("_gameStateState", JToken.FromObject(value.State, serializer));
            o.Add("_gameStateFutureMove", JToken.FromObject(value.FutureMove, serializer));

            o.WriteTo(writer);
        }

        public override GameState<S, M> ReadJson(JsonReader reader, Type objectType, GameState<S, M> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            GameStage stage = obj["_gameStateStage"]
                .ToObject<GameStage>(new JsonSerializer { Converters = { new GameStageConverter() } });

            S state = obj["_gameStateState"].ToObject<S>(serializer);

            FutureMove<M> futureMove = obj["_gameStateFutureMove"]
                .ToObject<FutureMove<M>>(new JsonSerializer { Converters = { new FutureMoveConverter<M>() } });

            return new GameState<S, M> { State = state, FutureMove = futureMove, Stage = stage };
        }
    }

    public class FutureMove<T>
    {
        public T Move { get; set; }
        public DateTime DateTime { get; set; }
    }

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