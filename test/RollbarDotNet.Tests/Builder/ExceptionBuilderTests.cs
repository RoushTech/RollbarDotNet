namespace RollbarDotNet.Tests.Builder
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Payloads;
    using RollbarDotNet.Builder;
    using Xunit;
    using Exception = System.Exception;

    public class ExceptionBuilderTests
    {
        public ExceptionBuilderTests()
        {
            ExceptionBuilder = new ExceptionBuilder();
            Payload = new Payload();
        }

        protected ExceptionBuilder ExceptionBuilder { get; set; }

        protected Payload Payload { get; set; }

        protected void ThrowException(string a, int b)
        {
            throw new Exception("test exception");
        }


        [Fact]
        public void BuildsInnerException()
        {
            try
            {
                try
                {
                    try
                    {
                        throw new InvalidOperationException("inner exception2");
                    }
                    catch (Exception inner2)
                    {
                        throw new InvalidCastException("inner exception", inner2);
                    }
                }
                catch (Exception inner)
                {
                    throw new Exception("test exception", inner);
                }
            }
            catch (Exception exception)
            {
                ExceptionBuilder.Execute(Payload, exception);
                var body = Payload.Data?.Body;
                Assert.NotNull(body?.TraceChain);
                var first = body.TraceChain.First();
                Assert.Equal("Exception", first.Exception?.Class);
                Assert.Equal("test exception", first.Exception?.Message);

                var second = body.TraceChain.Skip(1).First();
                Assert.Equal("InvalidCastException", second.Exception?.Class);
                Assert.Equal("inner exception", second.Exception?.Message);
            }
        }

        [Fact]
        public void ExceptionCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => ExceptionBuilder.Execute(Payload, null));
        }

        [Fact]
        public void PayloadCannotBeNull()
        {
            try
            {
                throw new Exception("Test exception");
            }
            catch (Exception exception)
            {
                Assert.Throws<ArgumentNullException>(() => ExceptionBuilder.Execute(null, exception));
            }
        }

        [Fact]
        public void SetsPayload()
        {
            try
            {
                ThrowException("", 0);
            }
            catch (Exception exception)
            {
                ExceptionBuilder.Execute(Payload, exception);
                var payload = Payload.Data?.Body?.TraceChain?.FirstOrDefault();
                Assert.Equal("Exception", payload?.Exception?.Class);
                Assert.Equal("test exception", payload?.Exception?.Message);
                Assert.True(payload?.Frames?.Count == 2);
                var frame = payload?.Frames?.FirstOrDefault();
                Assert.Equal(
                    "RollbarDotNet.Tests.Builder.ExceptionBuilderTests.ThrowException(System.String a, System.Int32 b)",
                    frame?.Method);
                var stackTrace = new StackTrace(exception, true);
                var stackTraceFrame = stackTrace.GetFrames().FirstOrDefault();
                Assert.Equal(stackTraceFrame.GetFileColumnNumber(), frame?.ColumnNumber);
                Assert.Equal(stackTraceFrame.GetFileLineNumber(), frame?.LineNumber);
                Assert.Equal(stackTraceFrame.GetFileName(), frame?.Filename);
            }
        }
    }
}