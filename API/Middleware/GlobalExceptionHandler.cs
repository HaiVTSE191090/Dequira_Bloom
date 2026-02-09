using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            var (statusCode, message, errors) = exception switch
            {
                ValidationException validationEx => (
                    HttpStatusCode.BadRequest,
                    "Validation failed",
                    validationEx.Errors
                ),
                NotFoundException => (
                    HttpStatusCode.NotFound,
                    exception.Message,
                    null
                ),
                BadRequestException => (
                    HttpStatusCode.BadRequest,
                    exception.Message,
                    null
                ),
                UnauthorizedException => (
                    HttpStatusCode.Unauthorized,
                    exception.Message,
                    null
                ),
                _ => (
                    HttpStatusCode.InternalServerError,
                    "An internal server error occurred",
                    null
                )
            };

            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                status = statusCode,
                message,
                errors
            }, cancellationToken);

            return true;
        }
    }
}
