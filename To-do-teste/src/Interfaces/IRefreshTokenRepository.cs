using To_do_teste.src.Entities;
using Task = System.Threading.Tasks.Task;

namespace To_do_teste.src.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetAsync(string refreshToken);
        Task<List<RefreshToken>> GetByUserIdAsync(int userId);
        Task UpdateRevokedAtAsync(byte[] refreshToken);
        Task AddRefreshToken(RefreshToken refresh);
    }
}
