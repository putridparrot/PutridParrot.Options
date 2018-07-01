# PutridParrot.Options

# Optional<T>

_The Optional<T> class was originally written as a port of the [Java Optional](https://docs.oracle.com/javase/8/docs/api/java/util/Optional.html)
but re-written to use C# idioms and also with a touch of F# Option type ideas._

What is an Optional<T>?

Optional<T> acts as a container or wrapper around a value which may or may not have been supplied. The Optional<T>
supplies functionality which automativally checks the value exists and attempts to help reduce possible NullReferencExceptions. 

Notice I do not say whether the value is null or non-null because this implementation works on value types as well (such as Int32 etc.), 
hence you can differentiate between Optional<T> with or without a value, for value types this acts in some ways like a Nullable<T>. 
All this said, the primary use is to try to better deal with null values. 

# Examples

```
var optional = "Hello World".ToOptional();
var value = optional.Match(s => s.Length, 0);
```

The above is mean't to be similar to the F# match keyword against an F# Option type. Hence we first turn the string into an Optional<T>
and then using the Match method to run code based upon whether the Optional<T> IsSome (i.e. has a supplied value). In this example we return 
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

Whilst the Match method would also allow us to return the Value stored within the Optional<T>, we also have the ```GetValueOrDefault()```
method which, as per an Nullable<T>, will either return the Value or a default(T) or, if supplied, an alternate default value. This is safer 
that using the Value property which will throw an exception if you try to get the Value when non has been supplied.

_Note: Many of these methods can easily be replicated with If and other logic statements in C#, but using these methods should remove 
the need for such checks and supply more readable code._
