using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiatSharp.Models;
using Newtonsoft.Json;

namespace FiatSharp.JsonConverters
{
    public static class ConverterHelper
    {
        public static void AddConverters<Settings, State, Move>(IList<JsonConverter> converters)
        {
            converters.Add(
                new FutureMoveConverter<FutureMove<Move>>());
            converters
                .Add(new FiatPlayerConverter());
            converters
                .Add(new GameStageConverter());
            converters.Add(
                new InitialGameStateResultConverter<Settings, State, Move>());
            converters.Add(
                new GameStateConverter<State, Move>());
            converters.Add(
                new TupleConverter<Settings, GameState<State, Move>>());
        }
    }
}
