namespace RollbarDotNet.Tests.Builder
{
    using System;
    using Abstractions;
    using Moq;
    using Payloads;
    using RollbarDotNet.Builder;
    using Xunit;

    public class EnvironmentBuilderTests
    {
        [Fact]
        public void SetsPayload()
        {
            var dateTimeMoq = new Mock<IDateTime>();
            dateTimeMoq.Setup(d => d.UtcNow).Returns(new DateTime(2016, 6, 12, 2, 21, 6, DateTimeKind.Utc));
            var environmentBuilder = new EnvironmentBuilder(dateTimeMoq.Object);
            var payload = new Payload();
            environmentBuilder.Execute(payload);
            Assert.Equal(1465698066L, payload.Data?.Timestamp);
            Assert.Equal("C#", payload.Data?.Language);
            Assert.Equal(".NET Core", payload.Data?.Platform);
        }
    }
}