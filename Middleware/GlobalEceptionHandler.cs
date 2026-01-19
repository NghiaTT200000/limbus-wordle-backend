namespace Limbus_wordle_backend.Middleware
{
    public class GlobalExceptionHandler() : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Msg = "An unexpected error occurred.",
                    Details = ex.Message
                };

                var responseJson = System.Text.Json.JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}