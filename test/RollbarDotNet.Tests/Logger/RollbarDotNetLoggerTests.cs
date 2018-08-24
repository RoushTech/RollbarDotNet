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
        [MemberData(nameof(ExceptionLogs))]
        public void Log_SendsExceptionIfPassed(Exception ex, int rollbarExceptionCount)
        {
            this.Logger.Log(LogLevel.Error, 0, (string)null, ex, (s, e) => string.Empty);
            RollbarMock.Verify(r => r.SendException(It.IsAny<RollbarLevel>(), ex), Times.Exactly(rollbarExceptionCount));
        }

        [Theory]
        [MemberData(nameof(MessageLogs))]
        public void Log_SendsMessageIfNotPassedException(Exception ex, int rollbarExceptionCount)
        {
            this.Logger.Log(LogLevel.Error, 0, (string)null, ex, (s, e) => string.Empty);
            RollbarMock.Verify(r => r.SendMessage(It.IsAny<RollbarLevel>(), It.IsAny<string>()), Times.Exactly(rollbarExceptionCount));
        }

        public static IEnumerable<object[]> ExceptionLogs()
        {
            yield return new object[] { new Exception(), 1 };
            yield return new object[] { (Exception)null, 0 };
        }

        public static IEnumerable<object[]> MessageLogs()
        {
            yield return new object[] { new Exception(), 0 };
            yield return new object[] { (Exception)null, 1 };
        }
    }
}