using System.Data.Common;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            // 1. Log the error (In a real app, you'd log 'exception.Message' and 'exception.StackTrace' to a file or monitoring system)
            Console.WriteLine($"[Error] {exception.Message}"); // Log the error message
            Console.WriteLine($"[StackTrace] {exception.StackTrace}"); // Log the stack trace

            // 2. Set the response type to JSON
            context.Response.ContentType = "application/json";

            // 3. Determine the status code and error message based on the exception type
            var statusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                InvalidOperationException => (int)HttpStatusCode.BadRequest, // Query or transaction issues
                TaskCanceledException or TimeoutException => (int)HttpStatusCode.RequestTimeout, // Timeout
                DbUpdateException or DbException => (int)HttpStatusCode.InternalServerError, // ORM or SQL errors
                // InvalidOperationException => (int)HttpStatusCode.InternalServerError, // Missing environment variables
                JsonException or FormatException => (int)HttpStatusCode.BadRequest, // Serialization errors
                _ => (int)HttpStatusCode.InternalServerError,
            };

            var errorCode = exception switch
            {
                ArgumentNullException => "BAD_REQUEST",
                UnauthorizedAccessException => "UNAUTHORIZED",
                KeyNotFoundException => "NOT_FOUND",
                InvalidOperationException => "INVALID_OPERATION",
                TaskCanceledException or TimeoutException => "TIMEOUT",
                DbUpdateException or DbException => "DATABASE_ERROR",
                // InvalidOperationException => "CONFIGURATION_ERROR",
                JsonException or FormatException => "SERIALIZATION_ERROR",
                _ => "INTERNAL_SERVER_ERROR",
            };

            var message = exception switch
            {
                ArgumentNullException => "A required argument was null.",
                UnauthorizedAccessException => "You are not authorized to perform this action.",
                KeyNotFoundException => "The requested resource was not found.",
                InvalidOperationException => "An invalid operation occurred.",
                TaskCanceledException or TimeoutException =>
                    "The request timed out. Please try again later.",
                DbUpdateException or DbException =>
                    "A database error occurred. Please contact support.",
                // InvalidOperationException =>
                // "A required configuration is missing. Please check your environment variables.",
                JsonException or FormatException =>
                    "A serialization error occurred. Please check your input.",
                _ => "An unexpected error occurred. Please try again later.",
            };

            context.Response.StatusCode = statusCode;

            // 4. Create your consistent error object
            var response = new
            {
                success = false,
                statusCode, // Include the HTTP status code in the response
                errorCode,
                message,
            };

            // 5. Convert the object to a string and send it back to the user
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
