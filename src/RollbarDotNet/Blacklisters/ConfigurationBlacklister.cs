namespace RollbarDotNet.Blacklisters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Configuration;
    using Microsoft.Extensions.Options;

    public class ConfigurationBlacklister : IBlacklister
    {
        public BlacklistConfiguration Configuration { get; }

        public List<Regex> RegexChecks { get; }

        public List<string> StringChecks { get; }

        public ConfigurationBlacklister(IOptions<RollbarOptions> config)
        {
            this.Configuration = config?.Value?.Blacklist;
            this.RegexChecks = new List<Regex>();
            this.StringChecks = new List<string>();
            if (this.Configuration != null)
            {
                this.Configuration.Regex?.ForEach(r => this.RegexChecks.Add(new Regex(r)));
                this.Configuration.Text?.ForEach(t => this.StringChecks.Add(t));
            }
        }

        public bool Check(string name)
        {
            return this.RegexChecks.Any(c => c.IsMatch(name)) || this.StringChecks.Any(c => c == name);
        }
    }
}