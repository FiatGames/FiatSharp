using System;

namespace FiatSharp
{
    public class InitialGameStateResult<Settings,State,M>
    {
        public string Error { get; set; }
        public Tuple<Settings,GameState<State,M>> InitialGameState { get; set; }
    }
}