#if DOTNET
namespace RollbarDotNet.Core
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class ExceptionHandlerMiddleware
    {
        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            Rollbar rollbar)
        {
            this.Next = next;
            this.Rollbar = rollbar;
        }

        protected RequestDelegate Next { get; set; }
        
        protected Rollbar Rollbar { get; set; }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.Next(context);
            }
            catch(Exception exception)
            {
                var response = await this.Rollbar.SendException(exception);

                // Continue throwing up the stack, we just log, let someone else handle.
                throw;
            }
        }
    }
}
#endif