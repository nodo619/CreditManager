using CreditManager.Api.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace CreditManager.Api.Tests.Middleware;

public class RequestLoggingMiddlewareTests : IDisposable
{
    private readonly RequestLoggingMiddleware _middleware;
    private readonly HttpContext _context;
    private readonly StringWriter _logOutput;

    public RequestLoggingMiddlewareTests()
    {
        _logOutput = new StringWriter();
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.TextWriter(_logOutput, outputTemplate: "{Message}{NewLine}{Properties}{NewLine}")
            .CreateLogger();

        _middleware = new RequestLoggingMiddleware(next: (innerHttpContext) => Task.CompletedTask);
        _context = new DefaultHttpContext();
        _context.Request.Path = "/api/test";
        _context.Request.Method = "GET";
        _context.TraceIdentifier = "test-trace-id";
    }

    [Fact]
    public async Task InvokeAsync_WhenRequestSucceeds_ShouldLogStartAndEnd()
    {
        await _middleware.InvokeAsync(_context);

        var logOutput = _logOutput.ToString();
        logOutput.Should().Contain("HTTP \"GET\" \"/api/test\" started");
        logOutput.Should().Contain("HTTP \"GET\" \"/api/test\" completed");
        logOutput.Should().Contain("test-trace-id");
    }

    [Fact]
    public async Task InvokeAsync_WhenRequestFails_ShouldLogError()
    {
        var middleware = new RequestLoggingMiddleware(next: (innerHttpContext) =>
            throw new Exception("Test exception"));

        var act = () => middleware.InvokeAsync(_context);

        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");

        var logOutput = _logOutput.ToString();
        logOutput.Should().Contain("HTTP \"GET\" \"/api/test\" started");
        logOutput.Should().Contain("HTTP \"GET\" \"/api/test\" failed");
    }

    [Fact]
    public async Task InvokeAsync_ShouldIncludeRequestPropertiesInLogContext()
    {
        await _middleware.InvokeAsync(_context);

        var logOutput = _logOutput.ToString();
        logOutput.Should().Contain("RequestId: \"test-trace-id\"");
    }

    public void Dispose()
    {
        _logOutput.Dispose();
        Log.CloseAndFlush();
    }
}