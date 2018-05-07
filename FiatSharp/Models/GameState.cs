namespace FiatSharp.Models
{
    public class GameState<S, M>
    {
        public GameStage Stage { get; set; }
        public S State { get; set; }
        public FutureMove<M> FutureMove { get; set; }
    }
}