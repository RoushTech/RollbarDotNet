namespace RollbarDotNet.Builder
{
    using Microsoft.Extensions.Configuration;
    using Payloads;
    using System;
    public class ConfigurationBuilder : IBuilder
    {
        public ConfigurationBuilder(IConfigurationRoot configuration)
        {
            this.Configuration = configuration;
        }

        protected IConfigurationRoot Configuration { get; set; }

        public void Execute(Payload payload)
        {
            payload.AccessToken = this.Configuration["Rollbar:AccessToken"];
            payload.Data.Environment = this.Configuration["Rollbar:Environment"];

            if(string.IsNullOrEmpty(payload.AccessToken))
            {
                throw new InvalidOperationException("Configuration variable Rollbar:AccessToken must be set.");
            }

            if (string.IsNullOrEmpty(payload.Data.Environment))
            {
                throw new InvalidOperationException("Configuration variable Rollbar:Environment must be set.");
            }
        }
    }
}
