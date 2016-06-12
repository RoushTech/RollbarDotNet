namespace RollbarDotNet.Core
{
    using Abstractions;
    using Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddRollbar(this IServiceCollection services)
        {
            return services
                .AddSingleton<IBuilder, ConfigurationBuilder>()
                .AddSingleton<IBuilder, EnvironmentBuilder>()
                .AddSingleton<IBuilder, NotifierBuilder>()
                .AddSingleton<IDateTime, SystemDateTime>()
                .AddSingleton<IEnvironment, SystemEnvironment>()
                .AddScoped<Rollbar>();
        }

        public static IServiceCollection AddRollbarWeb(this IServiceCollection services)
        {
            return AddRollbar(services)
                .AddSingleton<IBuilder, ServerBuilder>()
                .AddScoped<IBuilder, RequestBuilder>();
        }
    }
}
