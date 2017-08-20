namespace RollbarDotNet.Builder
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Payloads;
    using Trace = Payloads.Trace;

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

            var traceChain = new List<Trace>();
            this.BuildTraceList(exception, traceChain);
            if(traceChain.Count > 0)
            {
                payload.Data.Body.TraceChain = traceChain;
            }
        }

        protected void BuildTraceList(System.Exception exception, List<Trace> traceList)
        {
            var trace = new Trace();
            trace.Exception = this.BuildException(exception);
            trace.Frames = this.BuildFrames(exception);
            traceList.Add(trace);
            if (exception.InnerException != null)
            {
                this.BuildTraceList(exception.InnerException, traceList);
            }
        }

        protected List<Frame> BuildFrames(System.Exception exception)
        {
            var frames = new List<Frame>();
            var stacktrace = new StackTrace(exception, true);
            foreach (var stackTraceFrame in stacktrace.GetFrames())
            {
                var frame = new Frame
                {
                    Filename = stackTraceFrame.GetFileName(),
                    ColumnNumber = stackTraceFrame.GetFileColumnNumber(),
                    LineNumber = stackTraceFrame.GetFileLineNumber(),
                    Method = stackTraceFrame.GetMethod()?.ToString()
                };
                frames.Add(frame);
            }

            if (exception.InnerException != null)
            {
                frames.AddRange(this.BuildFrames(exception.InnerException));
            }

            return frames;
        }

        protected Exception BuildException(System.Exception exception)
        {
            var payloadException = new Exception();
            payloadException.Class = exception.GetType().Name;
            payloadException.Message = exception.Message;
            return payloadException;
        }
    }
}
