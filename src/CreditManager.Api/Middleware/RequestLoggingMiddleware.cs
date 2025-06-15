using System.Diagnostics;
using Serilog;
using Serilog.Context;

namespace CreditManager.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        
        try
        {
            using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
            using (LogContext.PushProperty("RequestPath", context.Request.Path))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            {
                Log.Information("HTTP {RequestMethod} {RequestPath} started", 
                    context.Request.Method, 
                    context.Request.Path);

                await _next(context);

                sw.Stop();
                Log.Information("HTTP {RequestMethod} {RequestPath} completed in {ElapsedMilliseconds}ms with status code {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    sw.ElapsedMilliseconds,
                    context.Response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            sw.Stop();
            Log.Error(ex, "HTTP {RequestMethod} {RequestPath} failed after {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                sw.ElapsedMilliseconds);
            throw;
        }
    }
} 