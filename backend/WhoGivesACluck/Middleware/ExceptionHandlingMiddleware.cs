
using System.Net;
using System.Text.Json;

namespace WhoGivesACluck.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Middleware exception");

                var result = JsonSerializer.Serialize(new { message = e.Message, stack = e.StackTrace });

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}
