using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json.Converters;

namespace FiatSharp
{
    public class GameState<S, M>
    {
        public GameStage Stage { get; set; }
        public S State { get; set; }
        public FutureMove<M> FutureMove { get; set; }
    }
}