using System;

namespace FiatSharp
{
    public class FiatPlayer : IEquatable<FiatPlayer>
    {
        public bool IsSystem => Id == null;
        public int? Id { get; set; }

        public bool Equals(FiatPlayer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FiatPlayer) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}