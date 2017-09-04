namespace RollbarDotNet.Core
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class ExceptionHandlerMiddleware
    {
        public ExceptionHandlerMiddleware(
            RequestDelegate next)
        {
            this.Next = next;
        }

        protected RequestDelegate Next { get; set; }

        public async Task Invoke(HttpContext context, Rollbar rollbar)
        {
            try
            {
                await this.Next(context);
            }
            catch(Exception exception)
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
