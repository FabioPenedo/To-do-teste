using To_do_teste.src.DTOs;
using To_do_teste.src.Entities;

namespace To_do_teste.src.Interfaces
{
    public interface ITokenService
    {
        Task<Tokens> GenerateTokens(User user);
        Task<Tokens> RefreshAsync(string refreshToken);
    }
}
