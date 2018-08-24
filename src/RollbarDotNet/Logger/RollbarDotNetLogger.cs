namespace RollbarDotNet.Logger
{
    using System;
    using Configuration;
    using Microsoft.Extensions.Logging;

    public class RollbarDotNetLogger : ILogger
    {
        protected Rollbar Rollbar { get; }

        public RollbarDotNetLogger(Rollbar rollbar)
        {
            this.Rollbar = rollbar;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if(logLevel == LogLevel.None)
                return;

            var rollbarLogLevel = MapLogLevel(logLevel);
            if (exception != null)
            {
                this.Rollbar.SendException(rollbarLogLevel, exception).Wait();
                return;
            }

            this.Rollbar.SendMessage(rollbarLogLevel, formatter(state, exception)).Wait();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Critical || logLevel == LogLevel.Error || logLevel == LogLevel.Warning;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        private RollbarLevel MapLogLevel(LogLevel logLevel)
        {
            switch(logLevel)
            {
                case LogLevel.Debug:
                    return RollbarLevel.Debug;
                case LogLevel.Information:
                    return RollbarLevel.Info;
                case LogLevel.Warning:
                    return RollbarLevel.Warning;
                case LogLevel.Error:
                    return RollbarLevel.Error;
                case LogLevel.Critical:
                    return RollbarLevel.Critical;
                default:
                    return RollbarLevel.Debug;
            }
        }
    }
}