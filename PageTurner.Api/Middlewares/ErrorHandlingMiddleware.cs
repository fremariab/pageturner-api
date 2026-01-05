using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace PageTurner.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        // The 'next' delegate represents the next checkpoint in the pipeline
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Try to move to the next checkpoint (eventually the Controller)
                await _next(context);
            }
            catch (Exception ex)
            {
                // If ANYTHING fails above, catch it here!
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // 1. Log the error (In a real app, you'd log 'exception.Message' to a file)

            // 2. Set the response type to JSON
            context.Response.ContentType = "application/json";

            // 3. Determine the status code and error message based on the exception type
            var statusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorCode = exception switch
            {
                ArgumentNullException => "BAD_REQUEST",
                UnauthorizedAccessException => "UNAUTHORIZED",
                KeyNotFoundException => "NOT_FOUND",
                _ => "INTERNAL_SERVER_ERROR"
            };

            var message = exception switch
            {
                ArgumentNullException => "A required argument was null.",
                UnauthorizedAccessException => "You are not authorized to perform this action.",
                KeyNotFoundException => "The requested resource was not found.",
                _ => "An unexpected error occurred. Please try again later."
            };

            context.Response.StatusCode = statusCode;

            // 4. Create your consistent error object
            var response = new
            {
                success = false,
                errorCode,
                message
            };

            // 5. Convert the object to a string and send it back to the user
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
