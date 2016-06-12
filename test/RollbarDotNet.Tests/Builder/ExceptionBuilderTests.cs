namespace RollbarDotNet.Tests.Builder
{
    using Payloads;
    using RollbarDotNet.Builder;
    using Xunit;

    public class ExceptionBuilderTests
    {
        [Fact]
        public void SetsPayload()
        {
            var exceptionBuilder = new ExceptionBuilder();
            var payload = new Payload();
            try
            {
                throw new System.Exception("test exception");
            }
            catch(System.Exception exception)
            {
                exceptionBuilder.Execute(payload, exception);
                var payloadException = payload.Data?.Body?.Trace?.Exception;
                Assert.Equal("Exception", payloadException?.Class);
                Assert.Equal("test exception", payloadException?.Message);
                Assert.Equal(exception.StackTrace, payloadException?.Description);
            }
        }
    }
}
