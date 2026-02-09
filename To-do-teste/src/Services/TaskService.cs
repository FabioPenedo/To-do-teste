using System.Threading.Tasks;
using To_do_teste.src.DTOs;
using To_do_teste.src.Entities;
using To_do_teste.src.Exceptions;
using To_do_teste.src.Interfaces;
using To_do_teste.src.Logging;
using Task = System.Threading.Tasks.Task;
using TaskTodo = To_do_teste.src.Entities.Task;

namespace To_do_teste.src.Services
{
    public class TaskService(ITaskRepository task, ILogger<TaskService> logger)
    {
        private readonly ITaskRepository _taskRepository = task;
        private readonly ILogger<TaskService> _logger = logger;

        public async Task<TaskCreatedResponse> CreateTaskAsync(CreateTaskRequest task)
        {
            _logger.TaskCreationStarted(task.Title);
            var taskObject = new TaskTodo(task.Title, task.Description, task.IsCompleted, task.Category, task.UserId);

            var createdTask = await _taskRepository.AddAsync(taskObject);

            _logger.TaskCreated(createdTask.Title);

            return new TaskCreatedResponse(createdTask.Id, createdTask.Title, createdTask.Category);
        }

        public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllTasksAssync();

            return tasks.Select(t => new TaskResponse(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.Category,
                t.CreatedAt,
                t.UserName
            ));
        }

        public async Task<TaskResponse?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Task com ID {id} não encontrado");

            return new TaskResponse(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.Category,
                task.CreatedAt,
                task.UserName
            );
        }

        public async Task<TaskResponse?> GetTaskByUserAsync(string userName)
        {
            var task = await _taskRepository.GetByUserAsync(userName)
                    ?? throw new NotFoundException($"Task com usúario {userName} não encontrado");

            return new TaskResponse(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.Category,
                task.CreatedAt,
                task.UserName
            );
        }

        public async Task<IEnumerable<TaskResponse>> GetTaskByCategoryAsync(string category)
        {
            var tasks = await _taskRepository.GetByCategoryAsync(category)
                ?? throw new NotFoundException($"Task com categoria {category} não encontrado");

            return tasks.Select(t => new TaskResponse(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.Category,
                t.CreatedAt,
                t.UserName
            ));
        }

        public async Task<TaskResponse?> UpdateTaskAsync(int id, UpdateTaskRequest updateRequest)
        {
            var existingTask = await _taskRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Task com ID {id} não encontrado");

            existingTask = existingTask with
            {
                Title = updateRequest.Title ?? existingTask.Title,
                Description = updateRequest.Description ?? existingTask.Description,
                IsCompleted = updateRequest.IsCompleted ?? existingTask.IsCompleted,
                Category = updateRequest.Category ?? existingTask.Category
            };

            var TaskObject = new TaskTodo(existingTask.Id, existingTask.Title, existingTask.Description, existingTask.IsCompleted, existingTask.Category);

            var updatedTask = await _taskRepository.UpdateAsync(TaskObject);

            _logger.TaskUpdated(updatedTask.Title);

            return new TaskResponse(
                existingTask.Id,
                existingTask.Title,
                existingTask.Description,
                existingTask.IsCompleted,
                existingTask.Category,
                existingTask.CreatedAt,
                existingTask.UserName
            );
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Task com ID {id} não encontrado");

            await _taskRepository.DeleteAsync(id);

            _logger.TaskDeleted(task.Title);
        }

    }
}
