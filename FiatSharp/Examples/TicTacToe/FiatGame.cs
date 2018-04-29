using System;
using System.Collections.Generic;
using System.Linq;
using FiatSharp;

namespace FiatSharp.Examples.TicTacToe
{
    public class FiatGame : IFiatGame<Settings, State, Move, Settings,State>
    {
        public Settings DefaultSettings()
        {
            return new Settings
            {
                Players = new List<FiatPlayer>(),
                XPlayer = null,
                YPlayer = null,
                TimePerMove = TimeSpan.FromMinutes(1)
            };
        }

        public Settings AddPlayer(FiatPlayer player, Settings settings)
        {
            if (settings.Players.Count >= 2)
            {
                return null;
            }
            else
            {
                settings.Players.Add(player);
                return settings;
            }
        }

        public InitialGameStateResult<Settings, State, Move> InitialGameState(Settings settings)
        {
            if (settings.Players.Count < 2)
            {
                return new InitialGameStateResult<Settings, State, Move>
                {
                    Error = "Not Enough Players"
                };
            }
            else
            {
                settings.XPlayer = settings.Players[0];
                settings.XPlayer = settings.Players[1];
                return new InitialGameStateResult<Settings, State, Move>
                {
                    InitialGameState =
                        new Tuple<Settings, GameState<State, Move>>(settings,
                            new GameState<State, Move>
                            {
                                State = new State(),
                                Stage = GameStage.Playing,
                                FutureMove =
                                    new FutureMove<Move>
                                    {
                                        Move = new Forfeit(),
                                        DateTime = DateTime.Now + settings.TimePerMove
                                    }
                            })
                };
            }
        }

        public GameState<State, Move> MakeMove(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move)
        {
            move.MakeMove(state.State);
            if(state.State.Winner != null || state.State.Tied) state.Stage = GameStage.Done;
            if (state.Stage == GameStage.Playing)
                state.FutureMove = new FutureMove<Move>
                {
                    Move = new Forfeit(),
                    DateTime = DateTime.Now + settings.TimePerMove
                };
            return state;
        }

        public bool IsPlayersTurn(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move)
        {
            Player? p = null;
            if(settings.XPlayer == player) p = Player.X;
            if (settings.YPlayer == player) p = Player.O;
            return p == state.State.Turn;
        }

        public bool IsMoveValid(FiatPlayer player, Settings settings, GameState<State, Move> state, Move move)
        {
            if (move is Forfeit) return true;
            else if (move is PlaceMove)
            {
                PlaceMove pMove = (PlaceMove) move;
                return state.State.ValidMoves().OfType<PlaceMove>().Any(p => p.Spot == pMove.Spot);
            }
            else
            {
                return false;
            }
        }

        public Tuple<Settings, State> ToClientSettingsAndState(FiatPlayer player, Settings settings, GameState<State, Move> state)
        {
            return new Tuple<Settings, State>(settings, state.State);
        }
    }
}