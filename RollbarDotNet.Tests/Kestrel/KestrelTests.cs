namespace RollbarDotNet.Tests.Kestrel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using RollbarDotNet.Core;
    using RollbarDotNet.Logger;
    using Xunit;

    public class KestrelTests
    {
        public KestrelTests()
        {
            RollbarMock = new Mock<Rollbar>(null, null, null);
            RollbarMock.Setup(m => m.SendException(It.IsAny<RollbarLevel>(), It.IsAny<Exception>())).Returns(() => Task.FromResult<Payloads.Response>(null));
            RollbarMock.Setup(m => m.SendMessage(It.IsAny<RollbarLevel>(), It.IsAny<string>())).Returns(() => Task.FromResult<Payloads.Response>(null));
        }

        protected Mock<Rollbar> RollbarMock { get; set; }

        [Fact]
        public async Task CanAddRollbarDotNetLogger()
        {
            var webHost = CreateBuilder()
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

        private IWebHostBuilder CreateBuilder() => new WebHostBuilder()
            .UseDefaultServiceProvider(options =>
            {
                options.ValidateScopes = true;
            })
            .UseKestrel();
    }
}