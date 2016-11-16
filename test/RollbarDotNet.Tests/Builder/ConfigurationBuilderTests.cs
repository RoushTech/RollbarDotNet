namespace RollbarDotNet.Tests.Builder
{
    using Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Moq;
    using RollbarDotNet.Builder;
    using System;
    using Xunit;

    public class ConfigurationBuilderTests
    {
        protected const string ACCESSTOKEN = "Test access token";
        protected const string ENVIRONMENT = "Test";

        [Fact]
        public void Require_Access_Token()
        {
            var configurationMock = new Mock<IOptions<RollbarOptions>>();
            configurationMock.Setup(c => c.Value).Returns(new RollbarOptions());
            var configurationBuilder = new RollbarDotNet.Builder.ConfigurationBuilder(configurationMock.Object);
            var exception = Assert.Throws<InvalidOperationException>(() => configurationBuilder.Execute(new Payloads.Payload()));
            Assert.Equal("Configuration variable for your Rollbar AccessToken must be set (did you include services.Configure<RollbarOptions>?).", exception.Message);
        }

        [Fact]
        public void Require_Environment()
        {
            var configurationMock = new Mock<IOptions<RollbarOptions>>();
            configurationMock.Setup(c => c.Value).Returns(new RollbarOptions { AccessToken = ACCESSTOKEN });
            var configurationBuilder = new RollbarDotNet.Builder.ConfigurationBuilder(configurationMock.Object);
            var exception = Assert.Throws<InvalidOperationException>(() => configurationBuilder.Execute(new Payloads.Payload()));
            Assert.Equal("Configuration variable for your Rollbar Environment must be set (did you include services.Configure<RollbarOptions>?).", exception.Message);
        }
        
        [Fact]
        public void SetsPayload()
        {
            var configurationMock = new Mock<IOptions<RollbarOptions>>();
            configurationMock.Setup(c => c.Value).Returns(new RollbarOptions { AccessToken = ACCESSTOKEN, Environment = ENVIRONMENT });
            var configurationBuilder = new RollbarDotNet.Builder.ConfigurationBuilder(configurationMock.Object);
            var payload = new Payloads.Payload();
            configurationBuilder.Execute(payload);
            Assert.Equal(ACCESSTOKEN, payload.AccessToken);
            Assert.Equal(ENVIRONMENT, payload.Data?.Environment);
        }
    }
}
