using System;

namespace PutridParrot.Options
{
    /// <summary>
    /// Optional acts as a container for a type that may or may not be null. 
    /// 
    /// The code was developed to replicate the Java Optional class and then
    /// combined with some naming from F#'s Option along with additions in the 
    /// C# idioms etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Optional<T> : IEquatable<Optional<T>>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        public static readonly Optional<T> None = new Optional<T>();

        /// <summary>
        /// Creates an Optional with a default without
        /// a value
        /// </summary>
        public Optional()
        {
            _value = default(T);
            _hasValue = false;
        }

        /// <summary>
        /// Creates an Optional with the supplied non-null
        /// value
        /// </summary>
        /// <param name="value"></param>
        public Optional(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// Gets the value stored within the Optional.
        /// If the value is not present (i.e. null) a
        /// NullReferenceException is thrown
        /// </summary>
        public T Value
        {
            get
            {
                if (IsNone)
                {
                    throw new InvalidOperationException("No value present");
                }
                return _value;
            }
        }

        /// <summary>
        /// Gets the value from the Optional if it exists or returns
        /// the supplied default value of default of T if no arguments
        /// is supplied
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOfDefault(T defaultValue = default(T))
        {
            return IsSome ? Value : defaultValue;
        }

        /// <summary>
        /// Gets whether a value exists within the Optional
        /// </summary>
        public bool IsNone => !_hasValue;
        /// <summary>
        /// Gets whether some value exists within the Optional
        /// </summary>
        public bool IsSome => _hasValue;

        /// <summary>
        /// Compare's this optional with the supplied
        /// optional value or value argument, if the 
        /// argument is not an Optional then it's value
        /// os compared, hence you can compare an optional
        /// to another optional or a value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            var otherOptional = obj as Optional<T>;
            var otherValue = otherOptional != null ? otherOptional.Value : obj;
            var otherHasValue = otherOptional != null ? otherOptional._hasValue : otherValue != null;

            return Object.Equals(_value, otherValue) && _hasValue == otherHasValue;
        }

        public override int GetHashCode()
        {
            var hash = 23;
            hash = hash * 31 + (_value != null ? _value.GetHashCode() : 0);
            hash = hash * 31 + _hasValue.GetHashCode();
            return hash;
        }

        public bool Equals(Optional<T> other)
        {
            return Equals((object)other);
        }

        public static bool operator ==(Optional<T> l, Optional<T> r)
        {
            return ReferenceEquals(l, null) ?
                ReferenceEquals(r, null) :
                l.Equals((object)r);
        }

        public static bool operator !=(Optional<T> l, Optional<T> r)
        {
            return !(l == r);
        }

        public override string ToString()
        {
            return IsSome ? $"Optional[{_value}]" : "Optional.None";
        }
    }

}
