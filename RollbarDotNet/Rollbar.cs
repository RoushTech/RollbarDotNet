namespace RollbarDotNet
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Builder;
    using DotNext.Runtime.Caching;
    using Payloads;
    using Exception = System.Exception;

#nullable enable

    public class Rollbar
    {
        protected IEnumerable<IBuilder> Builders { get; }

        protected IEnumerable<IExceptionBuilder> ExceptionBuilders { get; }

        protected RollbarClient RollbarClient { get; }

        protected ConcurrentCache<Exception, Response> SentExceptions { get; } = new ConcurrentCache<Exception, Response>(100, CacheEvictionPolicy.LRU);

        public Rollbar(IEnumerable<IBuilder> builders,
            IEnumerable<IExceptionBuilder> exceptionBuilders,
            RollbarClient rollbarClient)
        {
            Builders = builders;
            ExceptionBuilders = exceptionBuilders;
            RollbarClient = rollbarClient;
        }

        public virtual Task<Response> SendException(Exception exception) => SendException(exception, null);

        public virtual async Task<Response> SendException(Exception exception, string? message)
        {
            return await SendException(RollbarLevel.Error, exception, message);
        }

        public virtual Task<Response> SendException(RollbarLevel level, Exception exception) => SendException(level, exception, null);

        public virtual async Task<Response> SendException(RollbarLevel level, Exception exception, string? message)
        {
            if (SentExceptions.TryGetValue(exception, out var cached))
            {
                return cached;
            }

            var payload = SetupPayload(level);
            foreach (var exceptionBuilder in ExceptionBuilders)
            {
                exceptionBuilder.Execute(payload, exception);
            }

            payload.Data.Title = message;
            var response = await RollbarClient.Send(payload);
            SentExceptions[exception] = response;
            return response;
        }

        public virtual async Task<Response> SendMessage(string message)
        {
            return await SendMessage(RollbarLevel.Info, message);
        }

        public virtual async Task<Response> SendMessage(RollbarLevel level, string message)
        {
            var payload = SetupPayload(level);
            payload.Data.Body.Message = new Message
            {
                Body = message
            };
            return await RollbarClient.Send(payload);
        }

        protected Payload SetupPayload(RollbarLevel level)
        {
            var payload = new Payload();
            payload.Data.Level = LevelToString(level);
            ExecuteBuilders(payload);
            return payload;
        }

        protected void ExecuteBuilders(Payload payload)
        {
            foreach (var builder in Builders)
            {
                builder.Execute(payload);
            }
        }

        protected static string LevelToString(RollbarLevel level)
        {
            return level.ToString().ToLower();
        }
    }
}
