using System.Net;
using Limbus_wordle_backend.Models.Exceptions;

namespace Limbus_wordle_backend.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception has occured.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    BaseException bex => (int)bex.statusCode,
                    _ => StatusCodes.Status500InternalServerError
                };

                var response = new
                {
                    message = ex.Message,
                    statusCode = context.Response.StatusCode
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}