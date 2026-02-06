using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using To_do_teste.src.DTOs;
using To_do_teste.src.Entities;
using To_do_teste.src.Interfaces;

namespace To_do_teste.src.Services
{
    public class TokenService(IOptions<JwtSettings> jwt, IRefreshTokenRepository refreshToken) : ITokenService
    {
        private readonly JwtSettings _jwt = jwt.Value;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshToken;

        public async Task<Tokens> GenerateTokens(User user)
        {
            var access = GenerateJwt(user);
            var refresh = await CreateRefreshToken(user.Id);

            return new Tokens(access, refresh, _jwt.ExpirationMinutes);
        }

        public async Task<Tokens> RefreshAsync(string refreshToken)
        {
            var rt = await _refreshTokenRepository.GetAsync(refreshToken);

            if (rt == null) throw new UnauthorizedAccessException();

            // Rotação
            await _refreshTokenRepository.UpdateRevokedAtAsync(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

            var newRefresh = await CreateRefreshToken(rt.UserId);
            var newAccess = GenerateJwt(rt.User);

            return new Tokens(newAccess, newRefresh, _jwt.ExpirationMinutes);
        }

        private string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> CreateRefreshToken(int userId)
        {
            //Buscar sessões ativas
            var activeSessions = await _refreshTokenRepository.GetByUserIdAsync(userId);

            //Se exceder o limite → revoga as mais antigas
            if (activeSessions.Count >= _jwt.MaxSessionsPerUser)
            {
                var sessionsToRevoke = activeSessions.Take(activeSessions.Count - _jwt.MaxSessionsPerUser + 1);

                foreach (var s in sessionsToRevoke)
                {
                    await _refreshTokenRepository.UpdateRevokedAtAsync(s.TokenHash);
                }

            }

            //Cria novo refresh token

            var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw));

            var refreshToken = new RefreshToken(hash, DateTime.UtcNow.AddDays(_jwt.ExpirationRefreshDays), userId);

            await _refreshTokenRepository.AddRefreshToken(refreshToken);
            return raw;
        }
    }
}
