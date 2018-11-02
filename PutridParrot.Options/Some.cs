namespace PutridParrot.Options
{
    public sealed class Some<T> : Option<T>
    {
        internal Some(T value)
        {
            Value = value;
        }

        public override T Value { get; }
    }
}
