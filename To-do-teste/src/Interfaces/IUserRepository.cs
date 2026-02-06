using To_do_teste.src.Entities;

namespace To_do_teste.src.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<User> AddAsync(User user);
    }
}
