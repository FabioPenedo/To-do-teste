using Microsoft.EntityFrameworkCore;
using To_do_teste.src.Data;
using To_do_teste.src.DTOs;
using To_do_teste.src.Interfaces;
using Task = System.Threading.Tasks.Task;
using TaskTodo = To_do_teste.src.Entities.Task;

namespace To_do_teste.src.Repositories
{
    public class TaskRepository(AppDbContext dbContext) : ITaskRepository
    {
        private readonly AppDbContext _db = dbContext;

        async public Task<TaskTodo> AddAsync(TaskTodo task)
        {
            await _db.Tasks.AddAsync(task);
            await _db.SaveChangesAsync();
            return task;
        }

        async public Task<TaskTodo> UpdateAsync(TaskTodo task)
        {
            await _db.Tasks
                .Where(t => t.Id == task.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.Title, task.Title)
                    .SetProperty(t => t.Description, task.Description)
                    .SetProperty(t => t.IsCompleted, task.IsCompleted)
                    .SetProperty(t => t.Category, task.Category)
                    .SetProperty(t => t.UpdatedAt, DateTime.UtcNow)
                    );

            return task;
        }

        async public Task<IEnumerable<TaskResponse>> GetAllTasksAssync()
        {
            return await _db.Tasks
                .Select(t => new TaskResponse(t.Id, t.Title, t.Description, t.IsCompleted, t.Category, t.CreatedAt, t.User.UserName))
                .AsNoTracking()
                .ToListAsync();
        }

        async public Task<TaskResponse?> GetByIdAsync(int id)
        {
            return await _db.Tasks
                .Where(t => t.Id == id)
                .Select(t => new TaskResponse(t.Id, t.Title, t.Description, t.IsCompleted, t.Category, t.CreatedAt, t.User.UserName))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        async public Task<TaskResponse?> GetByUserAsync(string userName)
        {
            return await _db.Tasks
                .Where(t => t.User.UserName.StartsWith(userName))
                .Select(t => new TaskResponse(t.Id, t.Title, t.Description, t.IsCompleted, t.Category, t.CreatedAt, t.User.UserName))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        async public Task<TaskResponse?> GetByCategoryAsync(string category)
        {
            return await _db.Tasks
                .Where(t => t.Category.StartsWith(category))
                .Select(t => new TaskResponse(t.Id, t.Title, t.Description, t.IsCompleted, t.Category, t.CreatedAt, t.User.UserName))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        async public Task DeleteAsync(int id)
        {
            await _db.Tasks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();
        }

    }
}
