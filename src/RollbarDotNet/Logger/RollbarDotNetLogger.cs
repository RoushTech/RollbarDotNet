namespace RollbarDotNet.Logger
{
    using System;
    using System.Threading.Tasks;
    using Configuration;
    using Microsoft.Extensions.Logging;

    public class RollbarDotNetLogger : ILogger
    {
        protected Rollbar Rollbar { get; }

        protected RollbarOptions RollbarOptions { get; }

        public RollbarDotNetLogger(Rollbar rollbar, RollbarOptions rollbarOptions)
        {
            this.Rollbar = rollbar;
            this.RollbarOptions = rollbarOptions;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (exception != null)
            {
                this.Rollbar.SendException(exception).Wait();
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
