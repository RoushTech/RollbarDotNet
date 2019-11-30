namespace RollbarDotNet.Tests
{
    using System.Collections.Generic;
    using Moq;
    using RollbarDotNet.Blacklisters;
    using Xunit;

    public class BlacklistCollectionTests
    {
        [Fact]
        public void Check_Pass()
        {
            var blacklisterMock = new Mock<IBlacklister>();
            blacklisterMock.Setup(b => b.Check("test")).Returns(true);
            var blacklistCollection = new BlacklistCollection(new List<IBlacklister> { blacklisterMock.Object });
            Assert.True(blacklistCollection.Check("test"));
        }

        [Fact]
        public void CheckFails()
        {
            var blacklisterMock = new Mock<IBlacklister>();
            blacklisterMock.Setup(b => b.Check("test")).Returns(true);
            var blacklistCollection = new BlacklistCollection(new List<IBlacklister> { blacklisterMock.Object });
            Assert.False(blacklistCollection.Check("testa"));
        }
    }
}