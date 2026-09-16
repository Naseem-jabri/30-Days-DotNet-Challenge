using System.Text.Json;
using AuthNest_API.DTOS;

namespace AuthNest_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(
                    "Not Found: {Message}",
                    ex.Message);

                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status404NotFound,
                    ex.Message);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(
                    "Bad request: {Message}",
                    ex.Message);

                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning("Unauthorized: {Message}", ex.Message);

                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    ex.Message);
            }

            catch (ForbiddenException ex)
            {
                _logger.LogWarning("Forbidden: {Message}", ex.Message);

                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status403Forbidden,
                    ex.Message);
            }
            catch (ConflictException ex)
            {
                _logger.LogWarning(
                    "Conflict: {Message}",
                    ex.Message);

                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status409Conflict,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An unexpected error occurred.");

                await HandleExceptionAsync(context, ex);
            }

        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            await WriteErrorResponseAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.");
        }

        private static async Task WriteErrorResponseAsync(
            HttpContext context,
            int statusCode,
            string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponseDto
            {
                StatusCode = statusCode,
                Message = message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}