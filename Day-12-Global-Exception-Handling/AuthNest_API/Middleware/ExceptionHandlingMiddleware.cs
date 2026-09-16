using System.Text.Json;
using AuthNest_API.DTOS;

namespace AuthNest_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status404NotFound,
                    ex.Message);
            }
            catch (BadRequestException ex)
            {
                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    ex.Message);
            }

            catch (ForbiddenException ex)
            {
                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status403Forbidden,
                    ex.Message);
            }
            catch (ConflictException ex)
            {
                await WriteErrorResponseAsync(
                    context,
                    StatusCodes.Status409Conflict,
                    ex.Message);
            }
            catch (Exception ex)
            {
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