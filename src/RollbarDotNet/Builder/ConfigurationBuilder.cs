namespace RollbarDotNet.Builder
{
    using System;
    using Configuration;
    using Microsoft.Extensions.Options;
    using Payloads;

    public class ConfigurationBuilder : IBuilder
    {
        protected RollbarOptions Configuration { get; }

        public ConfigurationBuilder(IOptions<RollbarOptions> configuration)
        {
            this.Configuration = configuration.Value;
        }

        public void Execute(Payload payload)
        {
            payload.AccessToken = this.Configuration.AccessToken;
            payload.Data.Environment = this.Configuration.Environment;

            if (string.IsNullOrEmpty(payload.AccessToken))
            {
                throw new InvalidOperationException(
                    "Configuration variable for your Rollbar AccessToken must be set (did you include services.Configure<RollbarOptions>?).");
            }

            if (string.IsNullOrEmpty(payload.Data.Environment))
            {
                throw new InvalidOperationException(
                    "Configuration variable for your Rollbar Environment must be set (did you include services.Configure<RollbarOptions>?).");
            }
        }
    }
}