using To_do_teste.src.DTOs;
using Task = System.Threading.Tasks.Task;
using TaskTodo = To_do_teste.src.Entities.Task;

namespace To_do_teste.src.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskResponse>> GetAllTasksAssync();
        Task<TaskResponse?> GetByIdAsync(int id);
        Task<TaskResponse?> GetByUserAsync(string userName);
        Task<IEnumerable<TaskResponse>> GetByCategoryAsync(string category);
        Task<TaskTodo> AddAsync(TaskTodo task);
        Task<TaskTodo> UpdateAsync(TaskTodo task);
        Task DeleteAsync(int id);
    }
}
