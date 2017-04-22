namespace RollbarDotNet.Core
{
    using Abstractions;
    using Blacklisters;
    using Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

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
                .AddSingleton<IBlacklister, ConfigurationBlacklister>()
                .AddSingleton<IBlacklistCollection, BlacklistCollection>()
                .AddSingleton<IExceptionBuilder, ExceptionBuilder>()
                .AddSingleton<RollbarClient>()
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
