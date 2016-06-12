namespace RollbarDotNet.Tests.Builder
{
    using Microsoft.Extensions.Configuration;
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
            var configurationMock = new Mock<IConfigurationRoot>();
            var configurationBuilder = new ConfigurationBuilder(configurationMock.Object);
            var exception = Assert.Throws<InvalidOperationException>(() => configurationBuilder.Execute(new Payloads.Payload()));
            Assert.Equal("Configuration variable Rollbar:AccessToken must be set.", exception.Message);
        }

        [Fact]
        public void Require_Environment()
        {
            var configurationMock = new Mock<IConfigurationRoot>();
            configurationMock.Setup(c => c["Rollbar:AccessToken"]).Returns(ACCESSTOKEN);
            var configurationBuilder = new ConfigurationBuilder(configurationMock.Object);
            var exception = Assert.Throws<InvalidOperationException>(() => configurationBuilder.Execute(new Payloads.Payload()));
            Assert.Equal("Configuration variable Rollbar:Environment must be set.", exception.Message);
        }
        
        [Fact]
        public void SetsPayload()
        {
            var configurationMock = new Mock<IConfigurationRoot>();
            configurationMock.Setup(c => c["Rollbar:AccessToken"]).Returns(ACCESSTOKEN);
            configurationMock.Setup(c => c["Rollbar:Environment"]).Returns(ENVIRONMENT);
            var configurationBuilder = new ConfigurationBuilder(configurationMock.Object);
            var payload = new Payloads.Payload();
            configurationBuilder.Execute(payload);
            Assert.Equal(ACCESSTOKEN, payload.AccessToken);
            Assert.Equal(ENVIRONMENT, payload.Data?.Environment);
        }
    }
}
