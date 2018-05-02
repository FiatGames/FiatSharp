using System;
using System.Collections.Generic;
using FiatSharp;

namespace FiatSharp.Examples.TicTacToe
{
    public class Settings
    {
        public FiatPlayer XPlayer { get; set; }
        public FiatPlayer OPlayer { get; set; }
        public List<FiatPlayer> Players { get; set; }
        public TimeSpan TimePerMove { get; set; }
    }
}