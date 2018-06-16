namespace RollbarDotNet
{
    using System.Collections.Generic;
    using System.Linq;
    using Blacklisters;

    public class BlacklistCollection : IBlacklistCollection
    {
        protected List<IBlacklister> Blacklisters { get; }

        public BlacklistCollection(IEnumerable<IBlacklister> blacklisters)
        {
            this.Blacklisters = blacklisters.ToList();
        }

        public bool Check(string name)
        {
            return this.Blacklisters.Any(b => b.Check(name));
        }
    }
}