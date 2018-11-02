using System;

namespace PutridParrot.Options
{
    public sealed class None<T> : Option<T>
    {
        internal None()
        {
        }
        public override T Value => throw new InvalidOperationException("No value present");
    }
}