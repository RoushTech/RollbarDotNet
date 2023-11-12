namespace RollbarDotNet.Core
{
    public interface IRollbarResponseFeature
    {
        bool Handled { get; set; }
        string Uuid { get; set; }
    }
}