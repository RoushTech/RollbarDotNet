namespace RollbarDotNet.Builder
{
    using Payloads;

    public interface IExceptionBuilder
    {
        void Execute(Payload payload, System.Exception exception);
    }
}
