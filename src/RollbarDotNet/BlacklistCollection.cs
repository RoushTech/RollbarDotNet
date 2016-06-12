namespace RollbarDotNet
{
    using Blacklisters;
    using System.Collections.Generic;
    using System.Linq;

    public class BlacklistCollection : IBlacklistCollection
    {
        public BlacklistCollection(IEnumerable<IBlacklister> blacklisters)
        {
            this.Blacklisters = blacklisters.ToList();
        }

        protected List<IBlacklister> Blacklisters { get; set; }

        public bool Check(string name)
        {
            return this.Blacklisters.Any(b => b.Check(name));
        }
    }
}
