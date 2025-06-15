using System.Net;
using System.Text.Json;
using CreditManager.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace CreditManager.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var (statusCode, message) = exception switch
        {
            ValidationException validationException 
                => (HttpStatusCode.BadRequest, string.Join('\n', validationException.Errors.Select(e => e.ErrorMessage).ToList())),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "You are not authorized to access this resource"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "The requested resource was not found"),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = Result<Unit>.Failure(message);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}