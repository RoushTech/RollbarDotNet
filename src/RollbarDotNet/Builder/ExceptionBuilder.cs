namespace RollbarDotNet.Builder
{
    using System.Collections.Generic;
    using Payloads;

    public class ExceptionBuilder : IExceptionBuilder
    {
        public void Execute(Payload payload, System.Exception exception)
        {
            payload.Data.Body.Trace = new Trace();
            this.BuildException(payload.Data.Body.Trace.Exception, exception);
            this.BuildFrames(payload.Data.Body.Trace.Frames, exception);
        }

        private void BuildFrames(List<Frame> frames, System.Exception exception)
        {
            // Frames not supported by .NET Core yet
            // https://github.com/dotnet/corefx/issues/1797
        }

        protected void BuildException(Exception payload, System.Exception exception)
        {
            payload.Class = exception.GetType().Name;
            payload.Message = exception.Message;
            payload.Description = exception.StackTrace;
        }
    }
}
