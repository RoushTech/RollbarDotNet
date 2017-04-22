namespace RollbarDotNet.Tests.DependencyInjection
{
    using Configuration;
    using Core;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Threading.Tasks;
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

            this.Services = services;
            this.ServiceProvider = this.Services.BuildServiceProvider();
            this.Rollbar = this.ServiceProvider.GetService<Rollbar>();
        }

        protected IServiceCollection Services { get; set; }

        protected IServiceProvider ServiceProvider { get; set; }

        protected Rollbar Rollbar { get; set; }

        [Fact]
        public async Task SucessfullyReportError()
        {
            try
            {
                throw new Exception("Test");
            }
            catch(Exception exception)
            {
                var response = await this.Rollbar.SendException(exception);
                Assert.False(string.IsNullOrEmpty(response.Result.Uuid));
            }
        }

        [Fact]
        public async Task SuccessfullyReportMessage()
        {
            var response = await this.Rollbar.SendMessage("Hello");
            Assert.False(string.IsNullOrEmpty(response.Result.Uuid));
        }

        [Fact]
        public async Task SuccessfullyReportMessageWithLevel()
        {
            var response = await this.Rollbar.SendMessage(RollbarLevel.Debug, "Hello");
            Assert.False(string.IsNullOrEmpty(response.Result.Uuid));
        }

        [Fact]
        public async Task DisabledRollbar()
        {
            var options = this.ServiceProvider.GetService<IOptions<RollbarOptions>>().Value;
            this.Services.Configure<RollbarOptions>(o =>
            {
                o.AccessToken = options.AccessToken;
                o.Disabled = true;
                o.Environment = options.Environment;
            });
            var serviceProvider = this.Services.BuildServiceProvider();
            var rollbar = serviceProvider.GetService<Rollbar>();
            var response = await rollbar.SendMessage(RollbarLevel.Debug, "Hello");
            Assert.Null(response.Result.Uuid);
        }
    }
}
