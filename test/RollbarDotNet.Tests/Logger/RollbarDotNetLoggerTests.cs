namespace RollbarDotNet.Tests.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Configuration;
    using Core;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using RollbarDotNet.Logger;
    using Xunit;

    public class RollbarDotNetLoggerTests
    {
        public RollbarDotNetLoggerTests()
        {
            this.RollbarMock = new Mock<Rollbar>(null, null, null);
            this.RollbarMock.Setup(m => m.SendException(It.IsAny<RollbarLevel>(), It.IsAny<Exception>())).Returns(() => Task.FromResult<Payloads.Response>(null));
            this.RollbarMock.Setup(m => m.SendMessage(It.IsAny<RollbarLevel>(), It.IsAny<string>())).Returns(() => Task.FromResult<Payloads.Response>(null));
            this.Logger = new RollbarDotNetLogger(this.RollbarMock.Object);
        }

        protected Mock<Rollbar> RollbarMock { get; set; }
        protected ILogger Logger { get; set; }

        [Theory]
        [InlineData(LogLevel.Critical, true)]
        [InlineData(LogLevel.Error, true)]
        [InlineData(LogLevel.Warning, true)]
        [InlineData(LogLevel.Information, false)]
        [InlineData(LogLevel.Debug, false)]
        [InlineData(LogLevel.Trace, false)]
        [InlineData(LogLevel.None, false)]
        public void IsEnabled_Correctly(LogLevel logLevel, bool isEnabled)
        {
            Assert.Equal(isEnabled, this.Logger.IsEnabled(logLevel));
        }

        [Fact]
        public void BeginScope_ReturnsNull()
        {
            Assert.Null(this.Logger.BeginScope(null));
        }

        [Theory]
        [MemberData(nameof(Logs))]
        public void Log_SendsExceptionIfPassed(LogLevel logLevel, RollbarLevel rollbarLevel, Exception ex, bool shouldSendException, bool shouldSendMessage)
        {
            this.Logger.Log(logLevel, 0, (string)null, ex, (s, e) => string.Empty);
            RollbarMock.Verify(r => r.SendException(rollbarLevel, ex), Times.Exactly(shouldSendException ? 1 : 0));
            RollbarMock.Verify(r => r.SendMessage(rollbarLevel, It.IsAny<string>()), Times.Exactly(shouldSendMessage ? 1 : 0));
        }

        public static IEnumerable<object[]> Logs()
        {
            yield return new object[] { LogLevel.Critical, RollbarLevel.Critical, new Exception(), true, false };
            yield return new object[] { LogLevel.Critical, RollbarLevel.Critical, (Exception)null, false, true };
            yield return new object[] { LogLevel.Error, RollbarLevel.Error, new Exception(), true, false };
            yield return new object[] { LogLevel.Error, RollbarLevel.Error, (Exception)null, false, true };
            yield return new object[] { LogLevel.Warning, RollbarLevel.Warning, new Exception(), true, false };
            yield return new object[] { LogLevel.Warning, RollbarLevel.Warning, (Exception)null, false, true };
            yield return new object[] { LogLevel.Information, RollbarLevel.Info, new Exception(), true, false };
            yield return new object[] { LogLevel.Information, RollbarLevel.Info, (Exception)null, false, true };
            yield return new object[] { LogLevel.Debug, RollbarLevel.Debug, new Exception(), true, false };
            yield return new object[] { LogLevel.Debug, RollbarLevel.Debug, (Exception)null, false, true };
            yield return new object[] { LogLevel.Trace, RollbarLevel.Debug, new Exception(), true, false };
            yield return new object[] { LogLevel.Trace, RollbarLevel.Debug, (Exception)null, false, true };
            yield return new object[] { LogLevel.None, RollbarLevel.Debug, new Exception(), false, false };
            yield return new object[] { LogLevel.None, RollbarLevel.Debug, (Exception)null, false, false };
        }
    }
}