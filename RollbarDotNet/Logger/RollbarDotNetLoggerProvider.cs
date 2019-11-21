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
            Loggers = new ConcurrentDictionary<string, RollbarDotNetLogger>();
            ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
            Loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            var rollbar = ServiceProvider.GetRequiredService<Rollbar>();
            return Loggers.GetOrAdd(categoryName, name => new RollbarDotNetLogger(rollbar));
        }
    }
}