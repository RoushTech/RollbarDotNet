namespace RollbarDotNet
{
    public interface IBlacklistCollection
    {
        bool Check(string name);
    }
}
