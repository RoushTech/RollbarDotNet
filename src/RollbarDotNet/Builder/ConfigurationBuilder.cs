namespace RollbarDotNet.Builder
{
    using Configuration;
    using Microsoft.Extensions.Options;
    using Payloads;
    using System;

    public class ConfigurationBuilder : IBuilder
    {
        public ConfigurationBuilder(IOptions<RollbarOptions> configuration)
        {
            this.Configuration = configuration.Value;
        }

        protected RollbarOptions Configuration { get; set; }

        public void Execute(Payload payload)
        {
            payload.AccessToken = this.Configuration.AccessToken;
            payload.Data.Environment = this.Configuration.Environment;

            if(string.IsNullOrEmpty(payload.AccessToken))
            {
                throw new InvalidOperationException("Configuration variable for your Rollbar AccessToken must be set (did you include services.Configure<RollbarOptions>?).");
            }

            if (string.IsNullOrEmpty(payload.Data.Environment))
            {
                throw new InvalidOperationException("Configuration variable for your Rollbar Environment must be set (did you include services.Configure<RollbarOptions>?).");
            }
        }
    }
}
