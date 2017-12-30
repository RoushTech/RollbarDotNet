namespace RollbarDotNet.Tests.Builder
{
    using Payloads;
    using RollbarDotNet.Builder;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;
    using System.Reflection.PortableExecutable;
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

        protected delegate void ExceptionDelegate(string s1, int i1);

        [Fact]
        public void SetsPayload()
        {
            try
            {
                this.ThrowException("", 0);
            }
            catch (System.Exception exception)
            {
                this.ExceptionBuilder.Execute(this.Payload, exception);
                var payload = this.Payload.Data?.Body?.TraceChain?.FirstOrDefault();
                Assert.Equal("Exception", payload?.Exception?.Class);
                Assert.Equal("test exception", payload?.Exception?.Message);
                Assert.True(payload?.Frames?.Count == 2);
                var frame = payload.Frames?.FirstOrDefault();
                Assert.Equal(
                    "RollbarDotNet.Tests.Builder.ExceptionBuilderTests.ThrowException(System.String a, System.Int32 b)",
                    frame?.Method);
                var stackTrace = new StackTrace(exception, true);
                var stackTraceFrame = stackTrace.GetFrames().FirstOrDefault();
                Assert.NotNull(stackTraceFrame);
                Assert.Equal(stackTraceFrame.GetFileColumnNumber(), frame?.ColumnNumber);
                Assert.Equal(stackTraceFrame.GetFileLineNumber(), frame?.LineNumber);
                Assert.Equal(stackTraceFrame.GetFileName(), frame?.Filename);
            }
        }

        [Fact]
        public void HandlesDynamicMethods()
        {
            byte[] ilBytes;
            var methodInfo = typeof(ExceptionBuilderTests).GetMethod("ThrowException");
            var metadataToken = methodInfo.GetMetadataToken();
            using (var stream = File.OpenRead(methodInfo.DeclaringType.GetTypeInfo().Assembly.Location))
            using (var peReader = new PEReader(stream))
            {
                var metadataReader = peReader.GetMetadataReader();
                var methodHandle = MetadataTokens.MethodDefinitionHandle(metadataToken);
                var methodDef = metadataReader.GetMethodDefinition(methodHandle);
                var methodBody = peReader.GetMethodBody(methodDef.RelativeVirtualAddress);
                ilBytes = methodBody.GetILBytes();
            }
            
            

            var dynamicMethod = new DynamicMethod("ThrowException", null, new[] { typeof(string), typeof(string) },
                typeof(ExceptionBuilderTests).GetTypeInfo().Module);
            var dynamicMethodDelegate = (ExceptionDelegate)dynamicMethod.CreateDelegate(typeof(ExceptionDelegate));
            try
            {
                dynamicMethodDelegate(string.Empty, 0);
            }
            catch (System.Exception exception)
            {
                this.ExceptionBuilder.Execute(this.Payload, exception);
            }
        }
        
        protected void ThrowException(string a, int b)
        {
            throw new System.Exception("test exception");
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
                try
                {
                    try
                    {
                        throw new InvalidOperationException("inner exception2");
                    }
                    catch (System.Exception inner2)
                    {
                        throw new InvalidCastException("inner exception", inner2);
                    }
                }
                catch (System.Exception inner)
                {
                    throw new System.Exception("test exception", inner);
                }
            }
            catch (System.Exception exception)
            {
                this.ExceptionBuilder.Execute(this.Payload, exception);
                var body = this.Payload.Data?.Body;
                Assert.NotNull(body?.TraceChain);
                var first = body.TraceChain.First();
                Assert.Equal("Exception", first.Exception?.Class);
                Assert.Equal("test exception", first.Exception?.Message);

                var second = body.TraceChain.Skip(1).First();
                Assert.Equal("InvalidCastException", second.Exception?.Class);
                Assert.Equal("inner exception", second.Exception?.Message);
            }
        }
    }
}
