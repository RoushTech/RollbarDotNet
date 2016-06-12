namespace RollbarDotNet.Core
{
    using Abstractions;
    using Blacklisters;
    using Builder;
    using Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddRollbar(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddSingleton<IBuilder, Builder.ConfigurationBuilder>()
                .AddSingleton<IBuilder, EnvironmentBuilder>()
                .AddSingleton<IBuilder, NotifierBuilder>()
                .AddSingleton<IDateTime, SystemDateTime>()
                .AddSingleton<IEnvironment, SystemEnvironment>()
                .AddSingleton<IBlacklister, ConfigurationBlacklister>()
                .AddSingleton<IBlacklistCollection, BlacklistCollection>()
                .Configure<BlacklistConfiguration>(configuration.GetSection("Rollbar:Blacklist"))
                .AddScoped<Rollbar>();
        }

        public static IServiceCollection AddRollbarWeb(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return AddRollbar(services, configuration)
                .AddSingleton<IBuilder, ServerBuilder>()
                .AddScoped<IBuilder, RequestBuilder>();
        }
    }
}
