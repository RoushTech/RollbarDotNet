namespace RollbarDotNet
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Builder;
    using Payloads;
    using Exception = System.Exception;

    public class Rollbar
    {
        protected IEnumerable<IBuilder> Builders { get; }

        protected IEnumerable<IExceptionBuilder> ExceptionBuilders { get; }

        protected RollbarClient RollbarClient { get; }

        public Rollbar(IEnumerable<IBuilder> builders,
            IEnumerable<IExceptionBuilder> exceptionBuilders,
            RollbarClient rollbarClient)
        {
            this.Builders = builders;
            this.ExceptionBuilders = exceptionBuilders;
            this.RollbarClient = rollbarClient;
        }


        public virtual async Task<Response> SendException(Exception exception)
        {
            return await this.SendException(RollbarLevel.Error, exception);
        }

        public virtual async Task<Response> SendException(RollbarLevel level, Exception exception)
        {
            var payload = this.SetupPayload(level);
            foreach (var exceptionBuilder in this.ExceptionBuilders)
            {
                exceptionBuilder.Execute(payload, exception);
            }

            return await this.RollbarClient.Send(payload);
        }

        public virtual async Task<Response> SendMessage(string message)
        {
            return await this.SendMessage(RollbarLevel.Info, message);
        }

        public virtual async Task<Response> SendMessage(RollbarLevel level, string message)
        {
            var payload = this.SetupPayload(level);
            payload.Data.Body.Message = new Message();
            payload.Data.Body.Message.Body = message;
            return await this.RollbarClient.Send(payload);
        }

        protected Payload SetupPayload(RollbarLevel level)
        {
            var payload = new Payload();
            payload.Data.Level = this.LevelToString(level);
            this.ExecuteBuilders(payload);
            return payload;
        }

        protected void ExecuteBuilders(Payload payload)
        {
            foreach (var builder in this.Builders)
            {
                builder.Execute(payload);
            }
        }

        protected string LevelToString(RollbarLevel level)
        {
            return level.ToString().ToLower();
        }
    }
}