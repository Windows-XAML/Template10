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

            return th.Value ^ kh.Value;

        }
    }
