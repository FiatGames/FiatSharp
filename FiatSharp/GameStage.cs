using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiatSharp
{
    public enum GameStage { SettingUp, Playing, Done }

    public class GameStageConverter : JsonConverter<GameStage>
    {
        public override void WriteJson(JsonWriter writer, GameStage value, JsonSerializer serializer)
        {
            switch (value)
            {
                case GameStage.Done:
                    writer.WriteValue("Done");
                    break;
                case GameStage.Playing:
                    writer.WriteValue("Playing");
                    break;
                case GameStage.SettingUp:
                    writer.WriteValue("SettingUp");
                    break;
            }
        }

        public override GameStage ReadJson(JsonReader reader, Type objectType, GameStage existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "SettingUp":
                    return GameStage.SettingUp;
                case "Done":
                    return GameStage.Done;
                case "Playing":
                    return GameStage.Playing;
            }
            throw new Exception("Not valid GameStage JSON");
        }

    }
}