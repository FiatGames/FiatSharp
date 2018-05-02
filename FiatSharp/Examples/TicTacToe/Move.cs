using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace FiatSharp.Examples.TicTacToe
{
    public abstract class Move
    {
        public abstract void MakeMove(State s);
        public abstract JObject MakeJSON();
    }
    public class Forfeit : Move
    {
        public override void MakeMove(State s)
        {
            s.Winner = s.Turn == Player.O ? Player.X : Player.O;
        }

        public override JObject MakeJSON() => (JObject) JToken.FromObject(new {type = "Forfeit"});
    }
    public class PlaceMove : Move
    {
        public Spot Spot { get; set; }
        public override void MakeMove(State s)
        {
            if (s.Winner == null)
            {
                s.Board[Spot] = s.Turn;
                s.Winner = s.CheckWinner();
                if (s.Board.Values.All(v=>v.HasValue)) s.Tied = true;
                s.Turn = s.Turn == Player.O ? Player.X : Player.O;
            }
        }

        public override JObject MakeJSON() => (JObject) JToken.FromObject(new
        {
            type = "PlaceMove",
            spot = JsonConvert.SerializeObject(Spot,new StringEnumConverter())
        });
    }

    public class MoveConverter : JsonConverter<Move>
    {
        public override void WriteJson(JsonWriter writer, Move value, JsonSerializer serializer)
        {
            value.MakeJSON().WriteTo(writer);
        }

        public override Move ReadJson(JsonReader reader, Type objectType, Move existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            JObject obj = JObject.Load(reader);
            
            switch (obj["type"].ToObject<string>())
            {
                case "Forfeit":
                    return new Forfeit();
                case "PlaceMove":
                    return new PlaceMove{Spot = obj["spot"].ToObject<Spot>(new JsonSerializer{Converters = { new StringEnumConverter()}})};
            }
            throw new Exception("Can't read that move");
        }
    }
}