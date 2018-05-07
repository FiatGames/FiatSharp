using System;
using System.Collections.Generic;
using System.Linq;
using FiatSharp;
using FiatSharp.Models;
using FiatSharp.Models.Http;

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
                OPlayer = null,
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
                settings.OPlayer = settings.Players[1];
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
            if (state.State.Winner != null || state.State.Tied)
            {
                state.Stage = GameStage.Done;
                state.FutureMove = null;
            }
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
            if(settings.XPlayer.Equals(player)) p = Player.X;
            if (settings.OPlayer.Equals(player)) p = Player.O;
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

        public Tuple<Settings, GameState<State,Move>> ToClientSettingsAndState(FiatPlayer player, Settings settings, GameState<State, Move> state)
        {
            var s = state == null
                ? null
                : new GameState<State, Move> {State = state.State, FutureMove = state.FutureMove, Stage = state.Stage};
            return new Tuple<Settings, GameState<State, Move>>(settings,s);
        }
    }
}