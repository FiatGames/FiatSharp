using System;
using System.Web.Http;
using FiatSharp.Models;
using FiatSharp.Models.Http;

namespace FiatSharp.Controllers
{
    public abstract class FiatGameController<Settings,State,Move,ClientSettings,ClientState> : ApiController
    {
        public abstract IFiatGame<Settings, State, Move, ClientSettings, ClientState> GetFiatGame();

        [Route("~/defaultSettings")]
        public virtual Settings GetDefaultSettings()
        {
            return GetFiatGame().DefaultSettings();
        }

        [Route("~/addPlayer")]
        public virtual Settings PostAddPlayer([FromBody] AddPlayerRequest<Settings> req)
        {
            return GetFiatGame().AddPlayer(req.player, req.settings);
        }

        [Route("~/initialGameState")]
        public virtual InitialGameStateResult<Settings,State,Move> PostInitialGameState([FromBody] Settings settings)
        {
            return GetFiatGame().InitialGameState(settings);
        }

        [Route("~/makeMove")]
        public virtual GameState<State, Move> PostMakeMove([FromBody] MakeMoveRequest<Settings, State, Move> req)
        {
            return GetFiatGame().MakeMove(req.player, req.settings, req.state, req.move);
        }

        [Route("~/isPlayersTurn")]
        public virtual bool PostIsPlayersTurn([FromBody] MakeMoveRequest<Settings, State, Move> req)
        {
            return GetFiatGame().IsPlayersTurn(req.player, req.settings, req.state, req.move);
        }

        [Route("~/isMoveValid")]
        public virtual bool PostIsMoveValid([FromBody] MakeMoveRequest<Settings, State, Move> req)
        {
            return GetFiatGame().IsMoveValid(req.player, req.settings, req.state, req.move);
        }

        [Route("~/toClientSettingsAndState")]
        public virtual Tuple<ClientSettings,GameState<ClientState,Move>> PostToClientSettingsAndState([FromBody] MakeMoveRequest<Settings, State, Move> req)
        {
            return GetFiatGame().ToClientSettingsAndState(req.player, req.settings, req.state);
        }
    }
}
