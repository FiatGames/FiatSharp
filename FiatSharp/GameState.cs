using System;

namespace FiatSharp
{
    public class GameState<S, M>
    {
        public GameStage Stage { get; set; }
        public S State { get; set; }
        public FutureMove<M> FutureMove { get; set; }
    }

    public class FutureMove<T>
    {
        public T Move { get; set; }
        public DateTime DateTime { get; set; }
    }
}