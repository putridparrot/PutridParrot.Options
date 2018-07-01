/// <summary>
/// Optional acts as a container for a type that may or may not be null. 
/// 
/// The code was developed to replicate the Java Optional class during 
/// some conversion of Java code to C# (which used Optional). However 
/// changes were made to adhere to C# idioms.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class Optional<T>
    where T : class
{
    private readonly T _value;

    private static readonly Optional<T> _empty = new Optional<T>();

    public Optional()
    {
        _value = null;
    }

    public Optional(T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        _value = value;
    }

    public static Optional<T> Empty()
    {
        return _empty;
    }

    public static Optional<T> Of(T value)
    {
        return new Optional<T>(value);
    }

    public static Optional<T> OfNullable(T value)
    {
        return value == null ? Empty() : new Optional<T>(value);
    }

    public T Get()
    {
        if (_value == null)
        {
            throw new NullReferenceException("No value present");
        }
        return _value;
    }

    public bool IsPresent()
    {
        return _value != null;
    }

    public void IfPresent(Action<T> consumer)
    {
        if (IsPresent())
        {
            consumer(_value);
        }
    }

    public Optional<T> Filter(Predicate<T> predicate)
    {
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }
        return !IsPresent() ? this : (predicate(_value) ? this : Empty());
    }

    public Optional<U> Map<U>(Func<T, U> mapper)
        where U : class
    {
        if (mapper == null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (!IsPresent())
            return Optional<U>.Empty();

        return Optional<U>.OfNullable(mapper(_value));
    }

    public Optional<U> FlatMap<U>(Func<T, Optional<U>> mapper)
        where U : class
    {
        if (mapper == null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (!IsPresent())
            return Optional<U>.Empty();

        var result = mapper(_value);
        if (result == null)
        {
            throw new NullReferenceException("mapper result cannot be null");
        }
        return result;
    }

    public T OrElse(T other)
    {
        return _value ?? other;
    }

    public T OrElseGet(Func<T> other)
    {
        return _value ?? other();
    }

    public T OrElseThrow<X>(Func<X> exceptionFunc)
        where X : Exception
    {
        if (IsPresent())
            return _value;

        throw exceptionFunc();
    }

    public override bool Equals(object obj)
    {
        if (this == obj)
            return true;

        if (!(obj is Optional<T>))
            return false;

        var other = (Optional<T>)obj;
        return Object.Equals(_value, other._value);
    }

    public override int GetHashCode()
    {
        return IsPresent() ? _value.GetHashCode() : base.GetHashCode();
    }

    public override string ToString()
    {
        return IsPresent() ? $"Optional[{_value}]" : "Optional.Empty";
    }
}
