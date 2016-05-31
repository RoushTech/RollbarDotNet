namespace RollbarDotNet.Builder
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Payloads;

    public class ServerBuilder : IBuilder
    {
        public ServerBuilder(IHostingEnvironment hostingEnvironment)
        {
            this.HostingEnvironment = hostingEnvironment;
        }

        protected IHostingEnvironment HostingEnvironment { get; set; }

        public void Execute(Payload payload)
        {
            payload.Data.Server = new Server();
            this.BuildServer(payload.Data.Server);
        }

        private void BuildServer(Server server)
        {
            server.Host = Environment.MachineName;
            server.Root = this.HostingEnvironment.WebRootPath;
        }
    }
}
