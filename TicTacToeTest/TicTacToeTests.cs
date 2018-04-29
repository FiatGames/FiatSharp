using System;
using System.Collections.Generic;
using System.Linq;
using FiatSharp.Examples.TicTacToe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToeTest
{
    [TestClass]
    public class TicTacToeTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var g = new State();
            var mvs = new List<Move>
            {
                new PlaceMove {Spot = Spot.LL},
                new PlaceMove {Spot = Spot.ML},
                new PlaceMove {Spot = Spot.LM},
                new PlaceMove {Spot = Spot.MM},
                new PlaceMove {Spot = Spot.LR}
            };

            foreach (var m in mvs)
            {
                m.MakeMove(g);
            }
            
            Assert.IsTrue(g.Board[Spot.LL] == Player.X);
            Assert.IsTrue(g.Winner == Player.X);
            
        }
    }
}
