namespace RollbarDotNet.Tests.Blacklisters
{
    using System.Collections.Generic;
    using Configuration;
    using Microsoft.Extensions.Options;
    using Moq;
    using RollbarDotNet.Blacklisters;
    using Xunit;

    public class ConfigurationBlacklisterTests
    {
        protected ConfigurationBlacklister Setup()
        {
            var configurationBlacklistMock = new Mock<IOptions<RollbarOptions>>();
            configurationBlacklistMock.Setup(c => c.Value).Returns(new RollbarOptions
            {
                Blacklist = new BlacklistConfiguration
                {
                    Regex = new List<string> { "^rege.$" },
                    Text = new List<string> { "test" }
                }
            });
            return new ConfigurationBlacklister(configurationBlacklistMock.Object);
        }

        [Fact]
        public void RegexFail()
        {
            var configurationBlacklister = Setup();
            Assert.False(configurationBlacklister.Check("regexfail"));
        }

        [Fact]
        public void RegexPass()
        {
            var configurationBlacklister = Setup();
            Assert.True(configurationBlacklister.Check("regex"));
        }

        [Fact]
        public void StringFail()
        {
            var configurationBlacklister = Setup();
            Assert.False(configurationBlacklister.Check("testa"));
        }

        [Fact]
        public void StringPass()
        {
            var configurationBlacklister = Setup();
            Assert.True(configurationBlacklister.Check("test"));
        }
    }
}