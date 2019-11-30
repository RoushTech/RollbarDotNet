namespace RollbarDotNet.Tests.DependencyInjection
{
    using System;
    using System.Threading.Tasks;
    using Configuration;
    using Core;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class WebDependencyInjectionTests
    {
        public WebDependencyInjectionTests()
        {
            var mockHostingEnvironment = new Mock<IHostingEnvironment>();
            var services = new ServiceCollection();
            services.AddOptions()
                .AddRollbarWeb()
                .AddSingleton(mockHostingEnvironment.Object);
            services.Configure<RollbarOptions>(o =>
            {
                o.AccessToken = Environment.GetEnvironmentVariable("ROLLBAR_TOKEN");
                o.Environment = "Testing";
            });

            Services = services;
            ServiceProvider = Services.BuildServiceProvider();
            Rollbar = ServiceProvider.GetService<Rollbar>();
        }

        protected IServiceCollection Services { get; set; }

        protected IServiceProvider ServiceProvider { get; set; }

        protected Rollbar Rollbar { get; set; }

        [Fact]
        public async Task DisabledRollbar()
        {
            var options = ServiceProvider.GetService<IOptions<RollbarOptions>>().Value;
            Services.Configure<RollbarOptions>(o =>
            {
                o.AccessToken = options.AccessToken;
                o.Disabled = true;
                o.Environment = options.Environment;
            });
            var serviceProvider = Services.BuildServiceProvider();
            var rollbar = serviceProvider.GetService<Rollbar>();
            var response = await rollbar.SendMessage(RollbarLevel.Debug, "Hello");
            Assert.Null(response.Result.Uuid);
        }

        [Fact]
        public async Task SuccessfullyReportMessage()
        {
            var response = await Rollbar.SendMessage("Hello");
            Assert.False(string.IsNullOrEmpty(response.Result.Uuid));
        }

        [Fact]
        public async Task SuccessfullyReportMessageWithLevel()
        {
            var response = await Rollbar.SendMessage(RollbarLevel.Debug, "Hello");
            Assert.False(string.IsNullOrEmpty(response.Result.Uuid));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Test Message For Exception")]
        public async Task SuccessfullyReportError(string message)
        {
            try
            {
                try
                {
                    throw new Exception("Inner");
                }
                catch (Exception inner)
                {
                    throw new Exception("Test", inner);
                }
            }
            catch (Exception exception)
            {
                var response = await Rollbar.SendException(exception, message);
                Assert.False(string.IsNullOrEmpty(response.Result.Uuid));
            }
        }
    }
}