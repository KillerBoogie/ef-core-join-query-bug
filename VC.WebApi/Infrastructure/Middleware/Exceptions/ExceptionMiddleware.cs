using Serilog;
using System.Net;
using System.Text.Json;
using VC.WebApi.Shared.Errors;

namespace VC.WebApi.Infrastructure.Middleware.Exceptions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }

    public class ExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            logger.Error(exception, "Error during executing {Context}", context.Request.Path.Value);

            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await response.WriteAsync(
                JsonSerializer.Serialize(
                    new ProblemDetail(
                        type: "internal_server_error",
                        title: "An internal operation created an unexpected error.",
                        status: 500,
                        Guid.NewGuid().ToString(),
                        new Dictionary<string, object?> { { "detail", "We are sorry. The administrator has been informed. Please try again, later." } }
                    )
                )
            );
        }
    }
}
