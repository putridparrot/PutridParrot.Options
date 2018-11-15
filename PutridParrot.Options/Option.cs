using System;

namespace PutridParrot.Options
{
    public abstract class Option<T> 
    {
        public static readonly Option<T> None = new None<T>();
    }
}