using System;
using FiatSharp.Models.Http;

namespace FiatSharp.Models
{
    public interface IFiatGame<Settings, State, Move, ClientSettings, ClientState>
    {
        Settings DefaultSettings();
        Settings AddPlayer(FiatPlayer player, Settings settings);
        InitialGameStateResult<Settings, State, Move> InitialGameState(Settings settings);
        GameState<State, Move> MakeMove(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move);
        bool IsPlayersTurn(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move);
        bool IsMoveValid(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move);

        Tuple<ClientSettings, GameState<ClientState,Move>> ToClientSettingsAndState(FiatPlayer player, Settings settings,
            GameState<State, Move> state);

    }
}
