using System;

namespace PutridParrot.Options
{
    public sealed class Some<T> : Option<T>, IEquatable<Some<T>>
    {
        internal Some(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public override string ToString()
        {
            return this.IsSome() ? $"Option[{Value}]" : "Option.None";
        }

        public bool Equals(Some<T> other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            var otherOptional = obj as Some<T>;
            var otherValue = otherOptional != null ? otherOptional.Value : obj;
            var otherHasValue = otherOptional?.IsSome() ?? otherValue != null;

            return this.IsSome() == otherHasValue && Object.Equals(Value, otherValue);
        }

        public override int GetHashCode()
        {
            var hash = 23;
            hash = hash * 31 + (Value != null ? Value.GetHashCode() : 0);
            return hash;
        }
    }
}
