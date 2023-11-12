namespace RollbarDotNet.Core
{
    using System;
    using Microsoft.AspNetCore.Builder;

    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseRollbarExceptionHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}