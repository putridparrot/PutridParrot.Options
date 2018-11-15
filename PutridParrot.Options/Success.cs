namespace PutridParrot.Options
{
    public class Success<TSuccess, TFailure> : Result<TSuccess, TFailure>
    {
        public Success(TSuccess success)
        {
            Value = success;
        }

        public TSuccess Value { get; }
    }
}