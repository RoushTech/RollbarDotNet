namespace RollbarDotNet.Logger
{
    using System;
    using System.Collections.Concurrent;
    using Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class RollbarDotNetLoggerProvider : ILoggerProvider
    {
        public RollbarDotNetLoggerProvider(IServiceProvider serviceProvider)
        {
            this.Loggers = new ConcurrentDictionary<string, RollbarDotNetLogger>();
            this.ServiceProvider = serviceProvider;
        }

        protected ConcurrentDictionary<string, RollbarDotNetLogger> Loggers { get; }

        protected IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            this.Loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            var rollbar = this.ServiceProvider.GetRequiredService<Rollbar>();
            var options = ServiceProvider.GetRequiredService<RollbarOptions>();
            return this.Loggers.GetOrAdd(categoryName, name => new RollbarDotNetLogger(rollbar, options));
        }
    }
}
