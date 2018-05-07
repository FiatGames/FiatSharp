using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiatSharp.Models.Websocket
{
    public enum ToClientErrorType
    {
        GameIsNotStarted,
        GameAlreadyStarted,
        InvalidMove,
        Unauthorized,
        NotYourTurn,
        NotEnoughPlayers,
        DecodeError,
        FailedToInitialize,
        GameStateOutOfDate
    }

    public class ToClientCmd<Settings, State, Move>
    {
        public ToClientError Error { get; set; }
        public ToClientMsg<Settings,State,Move> Msg { get; set; }
    }

    public class ToClientError
    {
        public FiatPlayer Player { get; set; }
        public ToClientErrorType Error { get; set; }
        public string Msg { get; set; }
    }

    public class ToClientMsg<Settings, State, Move>
    {
        public FiatGameHash NewHash { get; set; }
        public Settings NewSettings { get; set; }
        public GameState<State,Move> NewGameState { get; set; }
    }
}
