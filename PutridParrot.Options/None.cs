using System;

namespace PutridParrot.Options
{
    public sealed class None<T> : Option<T>, IEquatable<None<T>>
    {
        internal None()
        {
        }

        public override string ToString()
        {
            return "Option.None";
        }

        public bool Equals(None<T> other)
        {
            return Equals((object) other);
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType().GetGenericTypeDefinition() == typeof(None<>);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}