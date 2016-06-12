namespace RollbarDotNet.Tests.Blacklisters
{
    using Microsoft.Extensions.Options;
    using Moq;
    using RollbarDotNet.Blacklisters;
    using Configuration;
    using System.Collections.Generic;
    using Xunit;

    public class ConfigurationBlacklisterTests
    {
        [Fact]
        public void StringPass()
        {
            var configurationBlacklister = this.Setup();
            Assert.Equal(true, configurationBlacklister.Check("test"));
        }

        [Fact]
        public void StringFail()
        {
            var configurationBlacklister = this.Setup();
            Assert.Equal(false, configurationBlacklister.Check("testa"));
        }

        [Fact]
        public void RegexPass()
        {
            var configurationBlacklister = this.Setup();
            Assert.Equal(true, configurationBlacklister.Check("regex"));
        }

        [Fact]
        public void RegexFail()
        {
            var configurationBlacklister = this.Setup();
            Assert.Equal(false, configurationBlacklister.Check("regexfail"));
        }

        protected ConfigurationBlacklister Setup()
        {
            var configurationBlacklistMock = new Mock<IOptions<BlacklistConfiguration>>();
            var blacklistConfigurationMock = new Mock<BlacklistConfiguration>();
            blacklistConfigurationMock.Setup(c => c.Text).Returns(new List<string>()
            {
                "test",
            });
            blacklistConfigurationMock.Setup(c => c.Regex).Returns(new List<string>()
            {
                "^rege.$"
            });

            configurationBlacklistMock.Setup(c => c.Value).Returns(blacklistConfigurationMock.Object);
            return new ConfigurationBlacklister(configurationBlacklistMock.Object);
        }
    }
}
