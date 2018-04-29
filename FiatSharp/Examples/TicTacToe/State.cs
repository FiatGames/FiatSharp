using System.Collections.Generic;
using System.Linq;

namespace FiatSharp.Examples.TicTacToe
{
    public enum Player { X, O }
    public enum Spot { UL, UM, UR, ML, MM, MR, LL, LM, LR }

    public class State
    {
        public Player Turn { get; set; }
        public Dictionary<Spot, Player?> Board { get; set; }
        public Player? Winner { get; set; }
        public bool Tied { get; set; }
        public State()
        {
            Turn = Player.X;
            Winner = null;
            Board = new Dictionary<Spot, Player?>
            {
                {Spot.UL, null}, {Spot.UM, null}, {Spot.UR, null},
                {Spot.ML, null}, {Spot.MM, null}, {Spot.MR, null},
                {Spot.LL, null}, {Spot.LM, null}, {Spot.LR, null}
            };
        }

        private static IEnumerable<HashSet<Spot>> _winningLines =
            new List<HashSet<Spot>>
            {
                new HashSet<Spot> {Spot.UL, Spot.UM, Spot.UR},
                new HashSet<Spot> {Spot.ML, Spot.MM, Spot.MR},
                new HashSet<Spot> {Spot.LL, Spot.LM, Spot.LR},
                new HashSet<Spot> {Spot.UL, Spot.ML, Spot.LL},

                new HashSet<Spot> {Spot.UM, Spot.MM, Spot.LM},
                new HashSet<Spot> {Spot.UL, Spot.ML, Spot.LL},

                new HashSet<Spot> {Spot.UL, Spot.MM, Spot.LR},
                new HashSet<Spot> {Spot.UR, Spot.ML, Spot.LR},

            };

        public Player? CheckWinner()
        {
            var xPieces = Board.Where(kvp => kvp.Value == Player.X).Select(kvp => (int)kvp.Key).ToList();
            var oPieces = Board.Where(kvp => kvp.Value == Player.O).Select(kvp => (int)kvp.Key).ToList();

            List<HashSet<int>> wInts = _winningLines.Select(w => new HashSet<int>(w.Cast<int>())).ToList();
            if(wInts.Any(w => xPieces.Permute(3).Any(w.SetEquals))) return Player.X;
            if (wInts.Any(w => oPieces.Permute(3).Any(w.SetEquals))) return Player.O;
            return null;
        }

        public List<Move> ValidMoves() => Winner == null
            ? Board.Aggregate(new List<Move>(), (l, kvp) =>
            {
                if (kvp.Value == null) l.Add(new PlaceMove { Spot = kvp.Key });
                return l;
            })
            : new List<Move>();
    }
}