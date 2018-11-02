using System;

namespace PutridParrot.Options
{
    public abstract class Option<T> : IEquatable<Option<T>>
    {
        public static readonly Option<T> None = new None<T>();

        public abstract T Value { get; }

        public override string ToString()
        {
            return this.IsSome() ? $"Option[{Value}]" : "Option.None";
        }

        public bool Equals(Option<T> other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            var otherOptional = obj as Option<T>;
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