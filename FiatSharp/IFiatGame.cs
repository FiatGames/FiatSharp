using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiatSharp
{
    public interface IFiatGame<Settings, State, Move, ClientSettings, ClientState>
    {
        Settings DefaultSettings();
        Settings AddPlayer(FiatPlayer player, Settings settings);
        InitialGameStateResult<Settings, State, Move> InitialGameState(Settings settings);
        GameState<State, Move> MakeMove(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move);
        bool IsPlayersTurn(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move);
        bool IsMoveValid(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move);

        Tuple<ClientSettings, ClientState> ToClientSettingsAndState(FiatPlayer player, Settings settings,
            GameState<State, Move> state);

    }
}
