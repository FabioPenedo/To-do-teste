using Serilog.Context;
using System.IdentityModel.Tokens.Jwt;
using To_do_teste.src.Context;

namespace To_do_teste.src.Middlewares
{
    public class ToDoMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            int? userId = null;

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (int.TryParse(userIdClaim, out var parsedUserId))
                {
                    userId = parsedUserId;
                }
            }

            // Configurar TenantContext
            ToDoContext.Set(userId, requestId);

            // Enriquecer Serilog LogContext com valores seguros para logs
            var logUserId = userId?.ToString() ?? "anonymous";

            using (LogContext.PushProperty("UserId", logUserId))
            using (LogContext.PushProperty("RequestId", requestId))
            {
                await _next(context);
            }

            ToDoContext.Clear();
        }
    }
}
