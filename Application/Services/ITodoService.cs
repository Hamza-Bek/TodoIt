using Application.Dtos.Todo;

namespace Application.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoDto>> GetTodosAsync();
    Task<TodoDto> GetTodoByIdAsync(Guid id);
    Task<TodoDto> AddTodoAsync(TodoDto todo);
    Task<TodoDto> UpdateTodoAsync(Guid id, TodoDto todo);
    Task DeleteTodoAsync(Guid id);
}