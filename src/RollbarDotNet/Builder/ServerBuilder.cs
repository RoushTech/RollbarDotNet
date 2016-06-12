namespace RollbarDotNet.Builder
{
    using Abstractions;
    using Microsoft.AspNetCore.Hosting;
    using Payloads;
    using System;

    public class ServerBuilder : IBuilder
    {
        public ServerBuilder(
            IEnvironment environment,
            IHostingEnvironment hostingEnvironment)
        {
            this.Environment = environment;
            this.HostingEnvironment = hostingEnvironment;
        }

        protected IEnvironment Environment { get; set; }

        protected IHostingEnvironment HostingEnvironment { get; set; }

        public void Execute(Payload payload)
        {
            payload.Data.Server = new Server();
            this.BuildServer(payload.Data.Server);
        }

        private void BuildServer(Server server)
        {
            server.Host = this.Environment.MachineName;
            server.Root = this.HostingEnvironment.WebRootPath;
        }
    }
}
