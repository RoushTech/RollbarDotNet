namespace RollbarDotNet.Core
{
    using Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddRollbar(this IServiceCollection services)
        {
            services.AddSingleton<IBuilder, ConfigurationBuilder>();
            services.AddSingleton<IBuilder, EnvironmentBuilder>();
            services.AddSingleton<IBuilder, NotifierBuilder>();
            services.AddScoped<Rollbar>();
            return services;
        }

        public static IServiceCollection AddRollbarWeb(this IServiceCollection services)
        {
            AddRollbar(services);
            services.AddSingleton<IBuilder, ServerBuilder>();
            services.AddScoped<IBuilder, RequestBuilder>();
            return services;
        }
    }
}
