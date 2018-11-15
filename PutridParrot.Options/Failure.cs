namespace PutridParrot.Options
{
    public class Failure<TSuccess, TFailure> : Result<TSuccess, TFailure>
    {
        public Failure(TFailure failure)
        {
            Value = failure;
        }

        public TFailure Value { get; }
    }
}