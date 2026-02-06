using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using To_do_teste.src.Data;
using To_do_teste.src.Entities;
using To_do_teste.src.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace To_do_teste.src.Repositories
{
    public class RefreshTokenRepository(AppDbContext dbContext) : IRefreshTokenRepository
    {
        private readonly AppDbContext _db = dbContext;

        public async Task AddRefreshToken(RefreshToken refresh)
        {
            await _db.RefreshTokens.AddAsync(refresh);
            await _db.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetAsync(string refreshToken)
        {
            var tokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
            return await _db.RefreshTokens
                .Include(r => r.User)
                .Where(r => r.RevokedAt == null &&
                    r.ExpiresAt > DateTime.UtcNow &&
                    r.TokenHash.SequenceEqual(tokenHash))
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<RefreshToken>> GetByUserIdAsync(int userId)
        {
            return await _db.RefreshTokens
                .Where(r =>
                    r.UserId == userId &&
                    r.RevokedAt == null &&
                    r.ExpiresAt > DateTime.UtcNow)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateRevokedAtAsync(byte[] refreshToken)
        {
            var rt = await _db.RefreshTokens.FirstOrDefaultAsync(r => r.TokenHash == refreshToken);
            if (rt != null) rt.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }
}
