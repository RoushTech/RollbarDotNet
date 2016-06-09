namespace RollbarDotNet.Core
{
    public interface IRollbarResponseFeature
    {
        string Uuid { get; set; }

        bool Handled { get; set; }
    }
}
