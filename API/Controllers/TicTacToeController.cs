using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using FiatSharp;
using FiatSharp.Examples.TicTacToe;

namespace API.Controllers
{
    public class TicTacToeController : FiatGameController<Settings,State,Move,Settings,State>
    {
        public override IFiatGame<Settings, State, Move, Settings, State> GetFiatGame() => new FiatGame();
    }
}
