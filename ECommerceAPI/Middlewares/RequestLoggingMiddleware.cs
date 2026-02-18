using System.Diagnostics;

namespace ECommerceAPI.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("➡️ İstek başladı: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            // Pipeline'daki bir sonraki adıma geç
            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation("✅ İstek tamamlandı: {Method} {Path} | Status: {StatusCode} | Süre: {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}