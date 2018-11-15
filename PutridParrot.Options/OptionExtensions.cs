using System;
using System.Net;

namespace PutridParrot.Options
{
    public static class OptionExtensions
    {
        public static Option<T> ToOption<T>(this T value)
        {
            return value == null ?
                Option<T>.None :
                new Some<T>(value);
        }

        public static bool IsSome<T>(this Option<T> option)
        {
            return option is Some<T>;
        }

        public static bool IsNone<T>(this Option<T> option)
        {
            return option is None<T>;
        }

        /// <summary>
        /// If the Option is Some then invoke the supplied action,
        /// otherwise do nothing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <param name="action"></param>
        public static void Do<T>(this Option<T> option, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (option is Some<T> some)
            {
                action(some.Value);
            }
        }

        /// <summary>
        /// If the Option has some value then call the
        /// supplied function to return a result else
        /// return a default/alternate value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="option"></param>
        /// <param name="someValue"></param>
        /// <param name="noneValue"></param>
        /// <returns></returns>
        public static TResult Match<T, TResult>(this Option<T> option,
            Func<T, TResult> someValue,
            TResult noneValue = default(TResult))
        {
            if (someValue == null)
            {
                throw new ArgumentNullException(nameof(someValue));
            }

            return option is Some<T> some ? someValue(some.Value) : noneValue;
        }

        /// <summary>
        /// If the Option has a value and the predicate returns true then 
        /// returns the Option otherwise returns a None
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Option<T> If<T>(this Option<T> option, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return option is Some<T> some && predicate(some.Value) ?
                option : Option<T>.None;
        }

        /// <summary>
        /// If the Option has a value and the supplied condition is true,
        /// returns the Option otherwise returns a None
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Option<T> If<T>(this Option<T> option, bool condition)
        {
            return option.IsSome() && condition ?
                option : Option<T>.None;
        }

        /// <summary>
        /// If the Option has a value then the value is returned otherwise
        /// the supplied "other" value is returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static T Or<T>(this Option<T> option, T other)
        {
            return option is Some<T> some ? some.Value : other;
        }

        /// <summary>
        /// If the Option has a value then the value is returned otherwise
        /// the supplied "other" function returns a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static T Or<T>(this Option<T> option, Func<T> other)
        {
            return option is Some<T> some ? some.Value : other();
        }

        /// <summary>
        /// If the Option has no value then the exceptionFunc
        /// is called to supply the Exception to be thrown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="option"></param>
        /// <param name="exceptionFunc">The function that returns an instance of the exception
        /// which will be thrown. Must not be null</param>
        /// <returns></returns>
        public static T OrException<T, TException>(this Option<T> option, Func<TException> exceptionFunc)
            where TException : Exception
        {
            if (exceptionFunc == null)
            {
                throw new ArgumentNullException(nameof(exceptionFunc));
            }

            switch (option)
            {
                case Some<T> some:
                    return some.Value;
                default:
                    throw exceptionFunc();
            }
        }

        /// <summary>
        /// If a value exists, apply the mapping function to it, otherwise
        /// return an empty Option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="option"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static Option<U> Map<T, U>(this Option<T> option,
            Func<T, U> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return option is Some<T> some ?
                mapper(some.Value).ToOption() :
                Option<U>.None;
        }

        /// <summary>
        /// If the value exists, apply the mapper to it, otherwise
        /// return an empty Option.
        /// 
        /// This is similar to Map except that the mapper returns an Option
        /// and if it returns null a NullReferenceException is thrown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="option"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static Option<U> Map<T, U>(this Option<T> option,
            Func<T, Option<U>> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            switch (option)
            {
                case Some<T> some:
                    var result = mapper(some.Value);
                    if (result == null)
                    {
                        throw new NullReferenceException("mapper result cannot be null");
                    }
                    return result;
                default:
                    return Option<U>.None;
            }
        }

        /// <summary>
        /// Casts the value within the Option to a supplied type. If
        /// the type conversion is not possible an InvalidCastException
        /// is thrown.
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Option<TTo> Cast<TFrom, TTo>(this Option<TFrom> option)
            where TTo : class
        {
            switch (option)
            {
                case Some<TFrom> some:
                    var cast = some.Value as TTo;
                    if (cast == null)
                    {
                        throw new InvalidCastException($"Cannot cast {some.Value.GetType().Name} to {typeof(TTo).Name}");
                    }
                    return cast.ToOption();
                default:
                    return Option<TTo>.None;
            }          
        }

        /// <summary>
        /// SafeCast is the same as a standard cast but no exception occurs, instead
        /// a None would be returned for a cast failure.
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Option<TTo> SafeCast<TFrom, TTo>(this Option<TFrom> option)
            where TTo : class
        {
            switch (option)
            {
                case Some<TFrom> some:
                    var cast = some.Value as TTo;
                    if (cast == null)
                    {
                        return Option<TTo>.None;
                    }
                    return cast.ToOption();
                default:
                    return Option<TTo>.None;
            }
        }

        /// <summary>
        /// Gets the value from the Option if it exists or returns
        /// the supplied default value of default of T if no arguments
        /// is supplied
        /// </summary>
        /// <param name="option"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValueOfDefault<T>(this Option<T> option, T defaultValue = default(T))
        {
            return option is Some<T> some ? some.Value : defaultValue;
        }
    }
}