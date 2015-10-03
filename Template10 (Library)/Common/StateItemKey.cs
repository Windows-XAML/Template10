using System;

namespace Template10.Common
{
    public struct StateItemKey : IEquatable<StateItemKey>
    {
        public Type Type { get; set; }
        public String Key { get; set; }

        public bool Equals(StateItemKey other)
        {
            return this.Type == other.Type && this.Key == other.Key;
        }

        public override int GetHashCode()
        {
            var th = Type?.GetHashCode();
            if (!th.HasValue)
            {
                throw new InvalidOperationException(nameof(Type) + " is null, cannot get hash");
            }
            var kh = Key?.GetHashCode();

            if (!kh.HasValue)
            {
                throw new InvalidOperationException(nameof(Key) + " is null, cannot get hash");
            }

            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + th.Value;
                hash = hash * 23 + kh.Value;
                return hash;
            }


        }
        public static bool operator ==(StateItemKey left, StateItemKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StateItemKey left, StateItemKey right)
        {
            return !left.Equals(right);
        }
    }

}
