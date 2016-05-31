namespace RollbarDotNet.Builder
{
    using Payloads;
    using System.Threading.Tasks;

    public interface IBuilder
    {
        void Execute(Payload payload);
    }
}
