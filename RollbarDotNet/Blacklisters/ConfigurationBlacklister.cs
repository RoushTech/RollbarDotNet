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
            Configuration = config?.Value?.Blacklist;
            RegexChecks = new List<Regex>();
            StringChecks = new List<string>();
            if (Configuration != null)
            {
                Configuration.Regex?.ForEach(r => RegexChecks.Add(new Regex(r)));
                Configuration.Text?.ForEach(t => StringChecks.Add(t));
            }
        }

        public bool Check(string name)
        {
            return RegexChecks.Any(c => c.IsMatch(name)) || StringChecks.Any(c => c == name);
        }
    }
}