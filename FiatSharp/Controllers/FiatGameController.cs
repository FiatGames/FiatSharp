﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FiatSharp;
using FiatSharp.Examples.TicTacToe;

namespace API.Controllers
{
    public abstract class FiatGameController<Settings,State,Move,ClientSettings,ClientState> : ApiController
    {
        public abstract IFiatGame<Settings, State, Move, ClientSettings, ClientState> GetFiatGame();

        [Route("~/defaultSettings")]
        public Settings GetDefaultSettings()
        {
            return GetFiatGame().DefaultSettings();
        }

        [Route("~/addPlayer")]
        public Settings PostAddPlayer([FromBody] AddPlayerRequest<Settings> req)
        {
            return GetFiatGame().AddPlayer(req.player, req.settings);
        }

        [Route("~/initialGameState")]
        public InitialGameStateResult<Settings,State,Move> PostInitialGameState([FromBody] Settings settings)
        {
            return GetFiatGame().InitialGameState(settings);
        }

        [Route("~/makeMove")]
        public GameState<State, Move> PostMakeMove([FromBody] MakeMoveRequest<Settings, State, Move> req)
        {
            return GetFiatGame().MakeMove(req.player, req.settings, req.state, req.move);
        }

        [Route("~/isPlayersTurn")]
        public bool PostIsPlayersTurn([FromBody] MakeMoveRequest<Settings, State, Move> req)
        {
            return GetFiatGame().IsPlayersTurn(req.player, req.settings, req.state, req.move);
        }

        [Route("~/isMoveValid")]
        public bool PostIsMoveValid([FromBody] MakeMoveRequest<Settings, State, Move> req)
        {
            return GetFiatGame().IsMoveValid(req.player, req.settings, req.state, req.move);
        }
    }
}