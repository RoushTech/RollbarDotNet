namespace RollbarDotNet.Configuration
{
    using System.Collections.Generic;

    public class BlacklistConfiguration
    {
        public virtual List<string> Text { get; set; }

        public virtual List<string> Regex { get; set; }
    }
}
