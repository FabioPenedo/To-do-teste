using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using To_do_teste.src.Configurations;
using To_do_teste.src.DTOs;
using To_do_teste.src.Services;

namespace To_do_teste.src.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(AuthService auth, IOptions<JwtSettings> jwt) : ControllerBase
    {
        private readonly AuthService _auth = auth;
        private readonly JwtSettings _jwt = jwt.Value;

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            var result = await _auth.Signup(request);

            Response.SetRefreshToken(result.token.RefreshToken, _jwt.ExpirationRefreshDays);

            return Ok(new
            {
                accessToken = result.token.AccessToken,
                user = result.User
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _auth.Login(request);

            Response.SetRefreshToken(result.token.RefreshToken, _jwt.ExpirationRefreshDays);

            return Ok(new
            {
                accessToken = result.token.AccessToken,
                user = result.User
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            {
                await _auth.Logout(refreshToken);
            }

            Response.ClearRefreshToken();
            return NoContent();
        }

    }
}
