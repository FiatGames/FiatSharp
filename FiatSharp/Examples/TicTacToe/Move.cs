using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace FiatSharp.Examples.TicTacToe
{
    public abstract class Move
    {
        public abstract void MakeMove(State s);
    }
    public class Forfeit : Move
    {
        public override void MakeMove(State s)
        {
            s.Winner = s.Turn == Player.O ? Player.X : Player.O;
        }
    }
    public class PlaceMove : Move
    {
        public Spot Spot { get; set; }
        public override void MakeMove(State s)
        {
            if (s.Winner == null)
            {
                s.Board[Spot] = s.Turn;
                s.Winner = s.CheckWinner();
                if (s.Board.Values.All(v=>v.HasValue)) s.Tied = true;
                s.Turn = s.Turn == Player.O ? Player.X : Player.O;
            }
        }
    }
}