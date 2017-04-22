namespace RollbarDotNet.Builder
{
    using System.Collections.Generic;
    using Payloads;

    public class ExceptionBuilder : IExceptionBuilder
    {
        public void Execute(Payload payload, System.Exception exception)
        {
            if(payload == null)
            {
                throw new System.ArgumentNullException(nameof(payload));
            }

            if(exception == null)
            {
                throw new System.ArgumentNullException(nameof(exception));
            }

            payload.Data.Body.Trace = new Trace();
            payload.Data.Body.Trace.Exception = this.BuildException(exception);
            payload.Data.Body.Trace.Frames = this.BuildFrames(exception);
            var traceChain = new List<Trace>();
            this.BuildTraceList(exception, traceChain);
            if(traceChain.Count > 0)
            {
                payload.Data.Body.TraceChain = traceChain;
            }
        }

        protected void BuildTraceList(System.Exception exception, List<Trace> traceList)
        {
            if(exception.InnerException != null)
            {
                var trace = new Trace();
                trace.Exception = this.BuildException(exception.InnerException);
                traceList.Add(trace);
                this.BuildTraceList(exception.InnerException, traceList);
            }
        }

        protected List<Frame> BuildFrames(System.Exception exception)
        {
            // Frames not supported by .NET Core yet
            // https://github.com/dotnet/corefx/issues/1797
            return new List<Frame>();
        }

        protected Exception BuildException(System.Exception exception)
        {
            var payloadException = new Exception();
            payloadException.Class = exception.GetType().Name;
            payloadException.Message = exception.Message;
            payloadException.Description = exception.StackTrace;
            return payloadException;
        }
    }
}
