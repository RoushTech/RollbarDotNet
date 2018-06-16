namespace RollbarDotNet.Blacklisters
{
    public interface IBlacklister
    {
        bool Check(string name);
    }
}