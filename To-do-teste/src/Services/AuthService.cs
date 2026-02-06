using System.Security.Cryptography;
using System.Text;
using To_do_teste.src.DTOs;
using To_do_teste.src.Entities;
using To_do_teste.src.Interfaces;
using To_do_teste.src.Logging;
using Task = System.Threading.Tasks.Task;

namespace To_do_teste.src.Services
{
    public class AuthService(
        ITokenService token,
        IRefreshTokenRepository refreshToken,
        IUserRepository user,
        ILogger<AuthService> logger)
    {
        private readonly ITokenService _token = token;
        private readonly IUserRepository _userRepository = user;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshToken;
        private readonly ILogger<AuthService> _logger = logger;

        public async Task<AuthResponse> Signup(SignupRequest request)
        {
            _logger.UserCreationStarted(request.UserName);

            var user = new User(request.UserName, BCrypt.Net.BCrypt.HashPassword(request.Password));

            await _userRepository.AddAsync(user);

            _logger.UserCreated(user.Id, user.UserName);

            var tokens = await _token.GenerateTokens(user);
            return new AuthResponse(
                new Tokens(tokens.AccessToken, tokens.RefreshToken, tokens.ExpiresIn),
                new UserDto(user.Id, user.UserName));
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByUserNameAsync(request.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LoginFailed(request.UserName);
                throw new UnauthorizedAccessException();
            }

            _logger.LoginSuccess(user.Id);

            var tokens = await _token.GenerateTokens(user);
            return new AuthResponse(
                new Tokens(tokens.AccessToken, tokens.RefreshToken, tokens.ExpiresIn),
                new UserDto(user.Id, user.UserName));
        }

        public async Task Logout(string refreshToken)
        {

            await _refreshTokenRepository.UpdateRevokedAtAsync(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
        }
    }
}
