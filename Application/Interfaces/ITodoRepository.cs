using Domain.Models;

namespace Application.Interfaces;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetTodosAsync();
    Task<Todo> GetTodoByIdAsync(Guid id);
    Task<Todo> AddTodoAsync(Todo todo);
    Task<Todo> UpdateTodoAsync(Guid id,Todo todo);
    Task<bool> DeleteTodoAsync(Guid id);
}