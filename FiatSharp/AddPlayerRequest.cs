namespace FiatSharp
{
    public class AddPlayerRequest<S>
    {
        public FiatPlayer player { get; set; }
        public S settings { get; set; }
    }
}
