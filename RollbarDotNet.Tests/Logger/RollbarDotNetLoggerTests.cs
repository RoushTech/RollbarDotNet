namespace RollbarDotNet.Tests.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using RollbarDotNet.Core;
    using RollbarDotNet.Logger;
    using Xunit;

    public class RollbarDotNetLoggerTests
    {
        public RollbarDotNetLoggerTests()
        {
            RollbarMock = new Mock<Rollbar>(null, null, null);
            RollbarMock.Setup(m => m.SendException(It.IsAny<RollbarLevel>(), It.IsAny<Exception>())).Returns(() => Task.FromResult<Payloads.Response>(null));
            RollbarMock.Setup(m => m.SendMessage(It.IsAny<RollbarLevel>(), It.IsAny<string>())).Returns(() => Task.FromResult<Payloads.Response>(null));
            Logger = new RollbarDotNetLogger(RollbarMock.Object);
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
            Assert.Equal(isEnabled, Logger.IsEnabled(logLevel));
        }

        [Fact]
        public void BeginScope_ReturnsNull()
        {
            Assert.Null(Logger.BeginScope(null));
        }

        [Fact]
        public async Task CanAddRollbarDotNetLogger()
        {
            var webHost = new WebHostBuilder()
                .UseDefaultServiceProvider(options =>
                {
                    options.ValidateScopes = true;
                })
                .UseKestrel()
                .ConfigureServices(services =>
                {
                    services.AddLogging().AddRollbar();
                })
                .Configure(app =>
                {
                    app.UseRollbarExceptionHandler();
                    var loggerFactory = app.ApplicationServices
                        .GetRequiredService<ILoggerFactory>();
                    loggerFactory.AddRollbarDotNetLogger(app.ApplicationServices);
                })
                .Build();
            await webHost.StartAsync();
            await webHost.StopAsync();
        }

        [Theory]
        [MemberData(nameof(Logs))]
        public void Log_SendsExceptionIfPassed(LogLevel logLevel, RollbarLevel rollbarLevel, Exception ex, bool shouldSendException, bool shouldSendMessage)
        {
            Logger.Log(logLevel, 0, (string)null, ex, (s, e) => string.Empty);
            RollbarMock.Verify(r => r.SendException(rollbarLevel, ex, It.IsAny<string>()), Times.Exactly(shouldSendException ? 1 : 0));
            RollbarMock.Verify(r => r.SendMessage(rollbarLevel, It.IsAny<string>()), Times.Exactly(shouldSendMessage ? 1 : 0));
        }

        public static IEnumerable<object[]> Logs()
        {
            yield return new object[] { LogLevel.Critical, RollbarLevel.Critical, new Exception(), true, false };
            yield return new object[] { LogLevel.Critical, RollbarLevel.Critical, null, false, true };
            yield return new object[] { LogLevel.Error, RollbarLevel.Error, new Exception(), true, false };
            yield return new object[] { LogLevel.Error, RollbarLevel.Error, null, false, true };
            yield return new object[] { LogLevel.Warning, RollbarLevel.Warning, new Exception(), true, false };
            yield return new object[] { LogLevel.Warning, RollbarLevel.Warning, null, false, true };
            yield return new object[] { LogLevel.Information, RollbarLevel.Info, new Exception(), true, false };
            yield return new object[] { LogLevel.Information, RollbarLevel.Info, null, false, true };
            yield return new object[] { LogLevel.Debug, RollbarLevel.Debug, new Exception(), true, false };
            yield return new object[] { LogLevel.Debug, RollbarLevel.Debug, null, false, true };
            yield return new object[] { LogLevel.Trace, RollbarLevel.Debug, new Exception(), true, false };
            yield return new object[] { LogLevel.Trace, RollbarLevel.Debug, null, false, true };
            yield return new object[] { LogLevel.None, RollbarLevel.Debug, new Exception(), false, false };
            yield return new object[] { LogLevel.None, RollbarLevel.Debug, null, false, false };
        }
    }
}