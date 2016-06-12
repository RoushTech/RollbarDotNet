namespace RollbarDotNet.Blacklisters
{
    using Configuration;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ConfigurationBlacklister : IBlacklister
    {
        public ConfigurationBlacklister(IOptions<BlacklistConfiguration> config)
        {
            this.Configuration = config?.Value;
            this.RegexChecks = new List<Regex>();
            this.StringChecks = new List<string>();
            if (this.Configuration != null)
            {
                this.Configuration.Regex?.ForEach(r => this.RegexChecks.Add(new Regex(r)));
                this.Configuration.Text?.ForEach(t => this.StringChecks.Add(t));
            }
        }

        public BlacklistConfiguration Configuration { get; set; }

        public List<string> StringChecks { get; set; }

        public List<Regex> RegexChecks { get; set; }

        public bool Check(string name)
        {
            return this.RegexChecks.Any(c => c.IsMatch(name)) || this.StringChecks.Any(c => c == name);
        }
    }
}
