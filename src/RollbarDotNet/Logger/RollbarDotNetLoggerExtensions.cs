namespace RollbarDotNet.Logger
{
    using System;
    using Microsoft.Extensions.Logging;

    public static class RollbarDotNetLoggerExtensions
    {
        public static ILoggerFactory AddRollbarDotNetLogger(this ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddProvider(new RollbarDotNetLoggerProvider(serviceProvider));
            return loggerFactory;
        }
    }
}
