namespace RollbarDotNet.Tests.DependencyInjection
{
    using Configuration;
    using Core;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
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
                await this.Rollbar.SendException(exception);
            }
        }

        [Fact]
        public async Task SuccessfullyReportMessage()
        {
            await this.Rollbar.SendMessage("Hello");
        }

        [Fact]
        public async Task SuccessfullyReportMessageWithLevel()
        {
            await this.Rollbar.SendMessage(RollbarLevel.Debug, "Hello");
        }
    }
}
