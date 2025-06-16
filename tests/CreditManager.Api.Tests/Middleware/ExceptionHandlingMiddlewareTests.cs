using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CreditManager.Api.Middleware;
using CreditManager.Application.Common.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace CreditManager.Api.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger;
    private readonly HttpContext _context;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExceptionHandlingMiddlewareTests()
    {
        _mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    private class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationException_ShouldReturnBadRequest()
    {
        var middleware = new ExceptionHandlingMiddleware(
            next: (innerHttpContext) => throw new FluentValidation.ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("Property", "Error message")
            }),
            _mockLogger.Object);

        await middleware.InvokeAsync(_context);

        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        _context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody();
        var result = JsonSerializer.Deserialize<ResultDto>(responseBody, _jsonOptions);

        result.Should().NotBeNull();
        result!.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error message");
    }

    [Fact]
    public async Task InvokeAsync_WhenUnauthorizedAccessException_ShouldReturnUnauthorized()
    {
        var middleware = new ExceptionHandlingMiddleware(
            next: (innerHttpContext) => throw new UnauthorizedAccessException(),
            _mockLogger.Object);

        await middleware.InvokeAsync(_context);

        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        _context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody();
        var result = JsonSerializer.Deserialize<ResultDto>(responseBody, _jsonOptions);

        result.Should().NotBeNull();
        result!.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("You are not authorized to access this resource");
    }

    [Fact]
    public async Task InvokeAsync_WhenKeyNotFoundException_ShouldReturnNotFound()
    {
        var middleware = new ExceptionHandlingMiddleware(
            next: (innerHttpContext) => throw new KeyNotFoundException(),
            _mockLogger.Object);

        await middleware.InvokeAsync(_context);

        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        _context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody();
        var result = JsonSerializer.Deserialize<ResultDto>(responseBody, _jsonOptions);

        result.Should().NotBeNull();
        result!.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("The requested resource was not found");
    }

    [Fact]
    public async Task InvokeAsync_WhenUnexpectedException_ShouldReturnInternalServerError()
    {
        var middleware = new ExceptionHandlingMiddleware(
            next: (innerHttpContext) => throw new Exception("Unexpected error"),
            _mockLogger.Object);

        await middleware.InvokeAsync(_context);

        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        _context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody();
        var result = JsonSerializer.Deserialize<ResultDto>(responseBody, _jsonOptions);

        result.Should().NotBeNull();
        result!.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("An unhandled exception has occurred")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    private async Task<string> GetResponseBody()
    {
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(_context.Response.Body);
        return await reader.ReadToEndAsync();
    }
}