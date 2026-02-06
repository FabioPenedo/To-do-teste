using Serilog;
using System.Diagnostics;
using To_do_teste.src.Context;

namespace To_do_teste.src.Middlewares
{
    public class LoggingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // Ignorar endpoints de swagger/
            var path = context.Request.Path.Value ?? "";
            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
                path.Equals("/", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            var method = context.Request.Method;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                var statusCode = context.Response.StatusCode;
                var elapsedMs = stopwatch.ElapsedMilliseconds;

                // Garantir que contexto está disponível para o log
                var userId = ToDoContext.UserId?.ToString() ?? "unknown";

                // Log estruturado com propriedades
                if (statusCode >= 500)
                {
                    Log.Error(
                        "http.request.completed HTTP request failed with server error HttpMethod={Method} Path={Path} StatusCode={StatusCode} ElapsedMs={ElapsedMs} UserId={UserId}",
                        method, path, statusCode, elapsedMs, userId);
                }
                else if (statusCode >= 400)
                {
                    Log.Warning(
                        "http.request.completed HTTP request completed with client error HttpMethod={Method} Path={Path} StatusCode={StatusCode} ElapsedMs={ElapsedMs} UserId={UserId}",
                        method, path, statusCode, elapsedMs, userId);
                }
                else if (elapsedMs > 1000)
                {
                    Log.Warning(
                        "http.request.completed HTTP request completed slowly HttpMethod={Method} Path={Path} StatusCode={StatusCode} ElapsedMs={ElapsedMs} UserId={UserId}",
                        method, path, statusCode, elapsedMs, userId);
                }
                else if (statusCode >= 200 && statusCode < 300)
                {
                    Log.Information(
                        "http.request.completed HTTP request completed successfully HttpMethod={Method} Path={Path} StatusCode={StatusCode} ElapsedMs={ElapsedMs} UserId={UserId}",
                        method, path, statusCode, elapsedMs, userId);
                }
            }
        }
    }
}
