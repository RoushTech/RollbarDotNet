namespace RollbarDotNet.Core
{
    using Abstractions;
    using Blacklisters;
    using Builder;
    using Configuration;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddRollbar(this IServiceCollection services)
        {
            return services
                .AddSingleton<IBuilder, Builder.ConfigurationBuilder>()
                .AddSingleton<IBuilder, EnvironmentBuilder>()
                .AddSingleton<IBuilder, NotifierBuilder>()
                .AddSingleton<IDateTime, SystemDateTime>()
                .AddSingleton<IEnvironment, SystemEnvironment>()
                .AddSingleton<IBlacklister, ConfigurationBlacklister>()
                .AddSingleton<IBlacklistCollection, BlacklistCollection>()
                .AddScoped<Rollbar>();
        }

        public static IServiceCollection AddRollbarWeb(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services.AddRollbar()
                .AddSingleton<IBuilder, ServerBuilder>()
                .AddScoped<IBuilder, RequestBuilder>()
                .AddScoped<IBuilder, PersonBuilder>();
        }
    }
}
