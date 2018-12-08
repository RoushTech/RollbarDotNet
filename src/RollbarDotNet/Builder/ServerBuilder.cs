namespace RollbarDotNet.Builder
{
    using Abstractions;
    using Microsoft.AspNetCore.Hosting;
    using Payloads;

    public class ServerBuilder : IBuilder
    {
        protected IEnvironment Environment { get; }

        protected IHostingEnvironment HostingEnvironment { get; }

        public ServerBuilder(
            IEnvironment environment,
            IHostingEnvironment hostingEnvironment)
        {
            this.Environment = environment;
            this.HostingEnvironment = hostingEnvironment;
        }

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