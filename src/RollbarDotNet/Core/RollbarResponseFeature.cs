namespace RollbarDotNet.Core
{
    public class RollbarResponseFeature : IRollbarResponseFeature
    {
        public bool Handled { get; set; }

        public string Uuid { get; set; }
    }
}
