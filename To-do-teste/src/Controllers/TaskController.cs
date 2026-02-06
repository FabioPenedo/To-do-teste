using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using To_do_teste.src.DTOs;
using To_do_teste.src.Services;

namespace To_do_teste.src.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
    public class TaskController(TaskService taskService) : ControllerBase
    {
        private readonly TaskService _taskService = taskService;

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _taskService.CreateTaskAsync(request);

            return Ok(ApiResponse<TaskCreatedResponse>.Ok(result, "Task criada com sucesso"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();

            return Ok(ApiResponse<IEnumerable<TaskResponse>>.Ok(tasks, "Tasks encontradas com sucesso"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            return Ok(ApiResponse<TaskResponse?>.Ok(task, "Task encontrada com sucesso"));
        }

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetTaskByUser(string userName)
        {
            var task = await _taskService.GetTaskByUserAsync(userName);

            return Ok(ApiResponse<TaskResponse?>.Ok(task, "Usuario filtrado com sucesso"));
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult> GetTaskByCategory(string category)
        {
            var task = await _taskService.GetTaskByCategoryAsync(category);

            return Ok(ApiResponse<TaskResponse?>.Ok(task, "Categoria filtrada com sucesso"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedTask = await _taskService.UpdateTaskAsync(id, request);

            return Ok(ApiResponse<TaskResponse?>.Ok(updatedTask, "Task atualizada com sucesso"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return Ok(ApiResponse<string>.Ok(string.Empty, "Usuário deletado com sucesso"));
        }
    }
}
