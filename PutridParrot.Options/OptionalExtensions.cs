using System;
using System.Collections.Generic;

namespace PutridParrot.Options
{
    /// <summary>
    /// Supplies extension methods for the Optional type
    /// </summary>
    public static class OptionalExtensions
    {
        /// <summary>
        /// If the Optional has a value then invoke the supplied action,
        /// otherwise do nothing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional"></param>
        /// <param name="action"></param>
        public static void Do<T>(this Optional<T> optional, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (optional.IsSome)
            {
                action(optional.Value);
            }
        }

        /// <summary>
        /// If the Optional has some value then call the
        /// supplied function to return a result else
        /// return a default/alternate value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="optional"></param>
        /// <param name="someValue"></param>
        /// <param name="noneValue"></param>
        /// <returns></returns>
        public static TResult Match<T, TResult>(this Optional<T> optional,
            Func<T, TResult> someValue,
            TResult noneValue = default(TResult))
        {
            if (someValue == null)
            {
                throw new ArgumentNullException(nameof(someValue));
            }

            return optional.IsSome ? someValue(optional.Value) : noneValue;
        }

        /// <summary>
        /// If the Optional has a value and the predicate returns true then 
        /// returns the Optional otherwise returns an Optional.None
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Optional<T> If<T>(this Optional<T> optional, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return optional.IsSome && predicate(optional.Value) ?
                optional : Optional<T>.None;
        }

        /// <summary>
        /// If the Optional has a value and the supplied condition is true,
        /// returns the Optional otherwise returns and Optional.None
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Optional<T> If<T>(this Optional<T> optional, bool condition)
        {
            return optional.IsSome && condition ?
                optional : Optional<T>.None;
        }

        /// <summary>
        /// If the Optional has a value then the value is returned otherwise
        /// the supplied "other" value is returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static T Or<T>(this Optional<T> optional, T other)
        {
            return optional.IsSome ? optional.Value : other;
        }

        /// <summary>
        /// If the Optional has a value then the value is returned otherwise
        /// the supplied "other" function returns a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static T Or<T>(this Optional<T> optional, Func<T> other)
        {
            return optional.IsSome ? optional.Value : other();
        }

        /// <summary>
        /// If the optional has no value then the exceptionFunc
        /// is called to supply the Exception to be thrown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="optional"></param>
        /// <param name="exceptionFunc">The function that returns an instance of the exception
        /// which will be thrown. Must not be null</param>
        /// <returns></returns>
        public static T OrException<T, TException>(this Optional<T> optional, Func<TException> exceptionFunc)
            where TException : Exception
        {
            if (exceptionFunc == null)
            {
                throw new ArgumentNullException(nameof(exceptionFunc));
            }

            if (optional.IsNone)
            {
                throw exceptionFunc();
            }

            return optional.Value;
        }

        /// <summary>
        /// If a value exists, apply the mapping function to it, otherwise
        /// return an empty Optional
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="optional"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static Optional<U> Map<T, U>(this Optional<T> optional,
            Func<T, U> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return optional.IsSome ?
                mapper(optional.Value).ToOptional() :
                Optional<U>.None;
        }

        /// <summary>
        /// If the value exists, apply the mapper to it, otherwise
        /// return an empty Optional.
        /// 
        /// This is similar to Map except that the mapper returns an Optional
        /// and if it returns null a NullReferenceException is thrown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="optional"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static Optional<U> Map<T, U>(this Optional<T> optional,
            Func<T, Optional<U>> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (optional.IsNone)
                return Optional<U>.None;

            var result = mapper(optional.Value);
            if (result == null)
            {
                throw new NullReferenceException("mapper result cannot be null");
            }
            return result;
        }

        /// <summary>
        /// Converts a supplied value to an optional. If the
        /// supplied value is null then an Optional.None is
        /// returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Optional<T> ToOptional<T>(this T value)
        {
            return value == null ? Optional<T>.None : new Optional<T>(value);
        }

        /// <summary>
        /// Casts the value within the optional to a supplied type. If
        /// the type conversion is not possible an InvalidCastException
        /// is thrown.
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static Optional<TTo> Cast<TFrom, TTo>(this Optional<TFrom> optional)
            where TTo : class
        {
            var cast = optional.Value as TTo;
            if (cast == null)
            {
                throw new InvalidCastException($"Cannot cast {optional.Value.GetType().Name} to {typeof(TTo).Name}");
            }
            return cast.ToOptional();
        }

        /// <summary>
        /// SafeCast is the same as a standard cast but no exception occurs, instead
        /// a Optional.None would be returned for a cast failure.
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static Optional<TTo> SafeCast<TFrom, TTo>(this Optional<TFrom> optional)
            where TTo : class
        {
            var cast = optional.Value as TTo;
            return cast.ToOptional();
        }
    }
}
