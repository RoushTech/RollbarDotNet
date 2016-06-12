namespace RollbarDotNet.Tests.Builder
{
    using Abstractions;
    using Microsoft.AspNetCore.Hosting;
    using Moq;
    using Payloads;
    using RollbarDotNet.Builder;
    using Xunit;

    public class ServerBuilderTests
    {
        [Fact]
        public void SetsPayload()
        {
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            hostingEnvironmentMock.Setup(h => h.WebRootPath).Returns("/root/path");
            var environmentMock = new Mock<IEnvironment>();
            environmentMock.Setup(e => e.MachineName).Returns("HostName");
            var serverBuilder = new ServerBuilder(environmentMock.Object, hostingEnvironmentMock.Object);
            var payload = new Payload();
            serverBuilder.Execute(payload);
            Assert.Equal("HostName", payload.Data?.Server?.Host);
            Assert.Equal("/root/path", payload.Data?.Server?.Root);
        }
    }
}
