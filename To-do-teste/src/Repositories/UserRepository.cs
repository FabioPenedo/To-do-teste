using Microsoft.EntityFrameworkCore;
using To_do_teste.src.Data;
using To_do_teste.src.Entities;
using To_do_teste.src.Interfaces;

namespace To_do_teste.src.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        private readonly AppDbContext _db = dbContext;

        async public Task<User> AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        async public Task<User?> GetByUserNameAsync(string userName)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
