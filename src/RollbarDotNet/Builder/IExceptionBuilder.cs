namespace RollbarDotNet.Builder
{
    using Payloads;
    using Exception = System.Exception;

    public interface IExceptionBuilder
    {
        void Execute(Payload payload, Exception exception);
    }
}