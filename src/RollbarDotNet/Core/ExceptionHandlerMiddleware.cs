namespace RollbarDotNet.Core
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class ExceptionHandlerMiddleware
    {
        protected RequestDelegate Next { get; set; }

        public ExceptionHandlerMiddleware(
            RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context, Rollbar rollbar)
        {
            try
            {
                await Next(context);
            }
            catch (Exception exception)
            {
                var response = await rollbar.SendException(exception);
                var rollbarResponseFeature = new RollbarResponseFeature
                {
                    Handled = true,
                    Uuid = response.Result.Uuid
                };
                context.Features.Set<IRollbarResponseFeature>(rollbarResponseFeature);

                // Continue throwing up the stack, we just log, let someone else handle.
                throw;
            }
        }
    }
}