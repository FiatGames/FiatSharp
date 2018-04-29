namespace FiatSharp
{
    public class FiatPlayer
    {
        public bool IsSystem => Id == null;
        public int? Id { get; set; }
    }
}