namespace FiatSharp
{
    public class MakeMoveRequest<Settings, State, M>
    {
        public FiatPlayer player { get; set; }
        public Settings settings { get; set; }
        public GameState<State, M> state { get; set; }
        public M move { get; set; }
    }
}
