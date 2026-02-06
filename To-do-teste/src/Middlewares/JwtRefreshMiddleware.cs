using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using To_do_teste.src.Configurations;
using To_do_teste.src.Data;
using To_do_teste.src.DTOs;
using To_do_teste.src.Interfaces;
using To_do_teste.src.Logging;

namespace To_do_teste.src.Middlewares
{
    public class JwtRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwt;
        private readonly ILogger<JwtRefreshMiddleware> _logger;

        public JwtRefreshMiddleware(
            RequestDelegate next,
            IOptions<JwtSettings> jwt,
            ILogger<JwtRefreshMiddleware> logger)
        {
            _next = next;
            _jwt = jwt.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            AppDbContext db,
            ITokenService tokenService)
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                _logger.LogCritical("auth.jwt.header.missing");
                _logger.JwtHeaderMissing();
                await _next(context);
                return;
            }

            var accessToken = authHeader["Bearer ".Length..].Trim();
            var handler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwt;
            try
            {
                jwt = handler.ReadJwtToken(accessToken);
            }
            catch (Exception ex)
            {
                _logger.JwtInvalid(ex);
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (jwt.ValidTo > DateTime.UtcNow)
            {
                _logger.JwtValid();
                await _next(context);
                return;
            }

            _logger.JwtExpired(jwt.ValidTo);
            _logger.JwtRefreshStarted();

            var refreshToken = context.Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                _logger.JwtRefreshMissingCookie();
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            try
            {
                var authResponse = await tokenService.RefreshAsync(refreshToken);

                context.Response.SetRefreshToken(authResponse.RefreshToken, _jwt.ExpirationRefreshDays);

                context.Response.Headers["X-New-Access-Token"] = authResponse.AccessToken;

                _logger.JwtRefreshSuccess();
                _logger.JwtNewAccessTokenSet();

                context.Request.Headers.Authorization = $"Bearer {authResponse.AccessToken}";

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.JwtRefreshFailed(ex);
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
    }
}
