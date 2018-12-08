namespace RollbarDotNet.Builder
{
    using Payloads;

    public interface IBuilder
    {
        void Execute(Payload payload);
    }
}