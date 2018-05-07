using System;
using FiatSharp.Models;
using FiatSharp.Models.Websocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp.JsonConverters
{
    public class ToClientCmdConverter<Settings, State, Move> : JsonConverter<ToClientCmd<Settings, State, Move>>
    {
        public override void WriteJson(JsonWriter writer, ToClientCmd<Settings, State, Move> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override ToClientCmd<Settings, State, Move> ReadJson(JsonReader reader, Type objectType, ToClientCmd<Settings, State, Move> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var ret = new ToClientCmd<Settings, State, Move>();
            if (reader.TokenType == JsonToken.Null) return null;
            JObject obj = JObject.Load(reader);
            switch (obj["tag"].ToObject<string>())
            {
                case "Error":
                    serializer.Converters.Add(new FiatPlayerConverter());
                    ret.Error = new ToClientError
                    {
                        Player = obj["player"].ToObject<FiatPlayer>(serializer),
                        Error = (ToClientErrorType)Enum.Parse(typeof(ToClientErrorType), obj["error"]["tag"].ToObject<string>()),
                        Msg = ((JObject)obj["error"]).ContainsKey("contents")
                            ? obj["error"]["contents"].ToObject<string>()
                            : null
                    };
                    break;
                case "Msg":
                    serializer.Converters.Add(new GameStateConverter<State, Move>());
                    ret.Msg = new ToClientMsg<Settings, State, Move>
                    {
                        NewSettings = obj["settings"].ToObject<Settings>(serializer),
                        NewGameState = obj["state"].Type == JTokenType.Null
                            ? null
                            : obj["state"].ToObject<GameState<State, Move>>(serializer),
                        NewHash = new FiatGameHash{ Hash = obj["hash"].ToObject<string>(serializer)}
                    };
                    break;
                default:
                    throw new Exception("Not valid FiatPlayer JSON");
            }
            return ret;
        }
    }
}