namespace RollbarDotNet.Tests.Builder
{
    using Payloads;
    using RollbarDotNet.Builder;
    using System;
    using System.Linq;
    using Xunit;

    public class ExceptionBuilderTests
    {
        public ExceptionBuilderTests()
        {
            this.ExceptionBuilder = new ExceptionBuilder();
            this.Payload = new Payload();
        }

        protected ExceptionBuilder ExceptionBuilder { get; set; }

        protected Payload Payload { get; set; }

        [Fact]
        public void SetsPayload()
        {
            try
            {
                throw new System.Exception("test exception");
            }
            catch (System.Exception exception)
            {
                this.ExceptionBuilder.Execute(this.Payload, exception);
                var payloadException = this.Payload.Data?.Body?.Trace?.Exception;
                Assert.Equal("Exception", payloadException?.Class);
                Assert.Equal("test exception", payloadException?.Message);
                Assert.Equal(exception.StackTrace, payloadException?.Description);
            }
        }

        [Fact]
        public void PayloadCannotBeNull()
        {
            try
            {
                throw new System.Exception("Test exception");
            }
            catch (System.Exception exception)
            {
                Assert.Throws<ArgumentNullException>(() => this.ExceptionBuilder.Execute(null, exception));
            }
        }

        [Fact]
        public void ExceptionCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => this.ExceptionBuilder.Execute(this.Payload, null));
        }


        [Fact]
        public void BuildsInnerException()
        {

            try
            {
                throw new System.Exception("test exception", new InvalidCastException("inner exception", new InvalidOperationException("inner exception2")));
            }
            catch (System.Exception exception)
            {
                this.ExceptionBuilder.Execute(this.Payload, exception);
                var body = this.Payload.Data?.Body;
                Assert.NotNull(body?.TraceChain);
                var first = body.TraceChain.First();
                Assert.Equal("InvalidCastException", first.Exception?.Class);
                Assert.Equal("inner exception", first.Exception?.Message);
                Assert.Equal(exception.InnerException.StackTrace, first.Exception?.Description);

                var second = body.TraceChain.Skip(1).First();
                Assert.Equal("InvalidOperationException", second.Exception?.Class);
                Assert.Equal("inner exception2", second.Exception?.Message);
                Assert.Equal(exception.InnerException.InnerException.StackTrace, second.Exception?.Description);
            }
        }
    }
}
