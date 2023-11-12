namespace RollbarDotNet.Tests.Kestrel
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using RollbarDotNet.Core;

    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRollbarWeb()
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseForwardedHeaders()
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors()
                .UseAuthentication()
                .UseAuthorization()
                .UseDefaultFiles()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}