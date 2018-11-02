# PutridParrot.Options

# Optional&lt;T&gt;

_The Optional&lt;T&gt; class was originally written as a port of the [Java Optional](https://docs.oracle.com/javase/8/docs/api/java/util/Optional.html)
but re-written to use C# idioms and also with a touch of F# Option type ideas._

What is an Optional&lt;T&gt;?

Optional&lt;T&gt; acts as a container or wrapper around a value which may or may not have been supplied. The Optional&lt;T&gt;
supplies functionality which automatically checks the value exists and attempts to help reduce possible NullReferencExceptions. 

Notice I do not say whether the value is null or non-null because this implementation works on value types as well (such as Int32 etc.), 
hence you can differentiate between Optional&lt;T&gt; with or without a value, for value types this acts in some ways like a Nullable&lt;T&gt;. 
All this said, the primary use is to try to better deal with null values. 

# Examples

```
var optional = "Hello World".ToOptional();
var value = optional.Match(s => s.Length, 0);
```

The above is mean't to be similar to the F# match keyword against an F# Option type. Hence we first turn the string into an Optional&lt;T&gt;
and then using the Match method to run code based upon whether the Optional&lt;T&gt; IsSome (i.e. has a supplied value). In this example we return 
the string's length, otherwise we return the value 0. As the second argument defaults to default(TResult) we can write the above without 
the second argument.

Ofcourse in a real world scenario we probably would not create an optional against a known non-null value, but we might instead
have something like the following

```
var optional = client.Read().ToOptional();
var value = optional.Match(s => s.Length, -1);
```

There will be no NullReferenceExceptions with this, so if Read() returned a null, it's turned into an Optional<T>.None and hence Match 
would return -1.

Whilst the Match method would also allow us to return the Value stored within the Optional&lt;T&gt;, we also have the ```GetValueOrDefault()```
method which, as per an Nullable&lt;T&gt;, will either return the Value or a default(T) or, if supplied, an alternate default value. This is safer 
that using the Value property which will throw an exception if you try to get the Value when none has been supplied.

_Note: Many of these methods can easily be replicated with If and other logic statements in C#, but using these methods should remove 
the need for such checks and supply more readable code._

# Option&lt;T&gt;

The Option class is made to be usable in pattern matching ways and so a little more similar to F#'s Option. Extension 
methods have been added duplicating Optional functionality to extend the functional capabilities.
