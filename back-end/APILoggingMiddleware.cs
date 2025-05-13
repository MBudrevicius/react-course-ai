using Serilog;
using System.Text;

public class APILoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger = Log.ForContext<APILoggingMiddleware>();

    public APILoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();

        string requestBody = string.Empty;
        if (context.Request.ContentLength > 0)
        {
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var logMessage = context.Request.Path.StartsWithSegments("/api/ai/transcribe", StringComparison.OrdinalIgnoreCase)
            ? "Received HTTP Request {Method} {Path}"
            : "Received HTTP Request {Method} {Path} | Body: \"{Body}\"";

        _logger.Information(logMessage,
            context.Request.Method,
            context.Request.Path,
            requestBody);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.Information("Sent out HTTP Response {StatusCode} | Body: \"{Body}\"",
            context.Response.StatusCode,
            responseText);

        await responseBody.CopyToAsync(originalBodyStream);
    }
}
