using System;

namespace FiatSharp.Models.Http
{
    public class InitialGameStateResult<Settings,State,Move>
    {
        public bool IsError => !String.IsNullOrEmpty(Error);
        public string Error { get; set; }
        public Tuple<Settings,GameState<State,Move>> InitialGameState { get; set; }
    }
}