namespace RollbarDotNet
{
    using Builder;
    using Payloads;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Rollbar
    {
        public Rollbar(IEnumerable<IBuilder> builders, 
                       IEnumerable<IExceptionBuilder> exceptionBuilders)
        {
            this.Builders = builders;
            this.ExceptionBuilders = exceptionBuilders;
            this.RollbarClient = new RollbarClient();
        }

        protected IEnumerable<IBuilder> Builders { get; set; }

        protected IEnumerable<IExceptionBuilder> ExceptionBuilders { get; set; }

        protected RollbarClient RollbarClient { get; set; }


        public async Task<Response> SendException(System.Exception exception)
        {
            return await this.SendException(RollbarLevel.Error, exception);
        }

        public async Task<Response> SendException(RollbarLevel level, System.Exception exception)
        {
            var payload = this.SetupPayload(level);
            foreach(var exceptionBuilder in this.ExceptionBuilders)
            {
                exceptionBuilder.Execute(payload, exception);
            }

            return await this.RollbarClient.Send(payload);
        }

        public async Task<Response> SendMessage(string message)
        {
            return await this.SendMessage(RollbarLevel.Info, message);
        }

        public async Task<Response> SendMessage(RollbarLevel level, string message)
        {
            var payload = this.SetupPayload(level);
            payload.Data.Body.Message.Body = message;
            return await this.RollbarClient.Send(payload);
        }

        protected Payload SetupPayload(RollbarLevel level)
        {
            var payload = new Payload();
            payload.Data.Level = LevelToString(level);
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
