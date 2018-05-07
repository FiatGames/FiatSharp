using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiatSharp.Models.Websocket
{
    public class ToServerCmd<Settings, Move>
    {
        public FiatPlayer Player { get; set; }
        public Cmd<Settings, Move> Cmd { get; set; }
        public FiatGameHash Hash { get; set; }
    }

    public abstract class Cmd<Settings, Move> { }
    public class StartGame<Settings, Move> : Cmd<Settings, Move> { }

    public class UpdateSettings<Settings, Move> : Cmd<Settings, Move>
    {
        public Settings SettingsUpdate { get; set; }
    }

    public class MakeMove<Settings, Move> : Cmd<Settings, Move>
    {
        public Move MoveToSubmit { get; set; }
    }
}
