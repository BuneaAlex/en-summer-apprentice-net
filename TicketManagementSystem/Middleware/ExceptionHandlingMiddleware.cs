using System.Net;
using System.Text.Json;
using TicketManagementSystem.Exceptions;

namespace TicketManagementSystem.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _nextRequestDelete;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate,ILogger<ExceptionHandlingMiddleware> logger)
        {
            _nextRequestDelete = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _nextRequestDelete(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            string message;
            switch(exception)
            {
                case EntityNotFoundException ex:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;

                case ArgumentNullException ex:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;

                case ArgumentException ex:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;

                default:
                    if(exception.Message.Contains("Invalid Token"))
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        message = exception.Message;
                        break;
                    }
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Internal server error!";
                    break;
               
            }

            _logger.LogError(exception.Message, exception.StackTrace);
            var result = JsonSerializer.Serialize(new { errorMessage = message });
            await httpContext.Response.WriteAsync(result);
        }
    }
}
