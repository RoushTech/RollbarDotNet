namespace RollbarDotNet.Logger
{
    using System;
    using System.Collections.Concurrent;
    using Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class RollbarDotNetLoggerProvider : ILoggerProvider
    {
        protected ConcurrentDictionary<string, RollbarDotNetLogger> Loggers { get; }

        protected IServiceProvider ServiceProvider { get; }

        public RollbarDotNetLoggerProvider(IServiceProvider serviceProvider)
        {
            this.Loggers = new ConcurrentDictionary<string, RollbarDotNetLogger>();
            this.ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
            this.Loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            var rollbar = this.ServiceProvider.GetRequiredService<Rollbar>();
            var options = this.ServiceProvider.GetRequiredService<IOptions<RollbarOptions>>();
            return this.Loggers.GetOrAdd(categoryName, name => new RollbarDotNetLogger(rollbar, options.Value));
        }
    }
}