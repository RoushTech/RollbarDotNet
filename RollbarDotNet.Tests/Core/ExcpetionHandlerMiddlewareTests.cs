namespace RollbarDotNet.Tests.Core
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Moq;
    using RollbarDotNet.Builder;
    using RollbarDotNet.Configuration;
    using RollbarDotNet.Core;
    using Xunit;

    public class ExceptionHandlerMiddlewareTests
    {

        [Fact]
        public async Task OnlyLogsOnce()
        {
            var uuid = string.Empty;
            var optionsMock = new Mock<IOptions<RollbarOptions>>();
            var rollbarClientMock = new Mock<RollbarClient>(optionsMock.Object);
            rollbarClientMock.SetupSequence(m => m.Send(It.IsAny<Payloads.Payload>()))
                .Returns(() => ReturnItem("test"))
                .Returns(() => ReturnItem("fail"));
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock
                .Setup(h => h.Features.Set(It.IsAny<IRollbarResponseFeature>()));
            var rollbar = new Rollbar(new IBuilder[] { }, new IExceptionBuilder[] { }, rollbarClientMock.Object);
            var middleware = new ExceptionHandlerMiddleware(context =>
            {
                try
                {
                    throw new Exception("Middleware tests");
                }
                catch (Exception exception)
                {
                    rollbar.SendException(exception).Wait();
                    throw;
                }
            });
            await Assert.ThrowsAsync<Exception>(async () => await middleware.Invoke(httpContextMock.Object, rollbar));
            rollbarClientMock.Verify(m => m.Send(It.IsAny<Payloads.Payload>()), Times.Once());
            httpContextMock.Verify(m => m.Features.Set(It.Is<IRollbarResponseFeature>(f => f.Uuid == "test")));
        }

        private Task<Payloads.Response> ReturnItem(string uuid)
        {
            return Task.FromResult(new Payloads.Response
            {
                Result = new Payloads.Result
                {
                    Uuid = uuid
                }
            });
        }
    }
}