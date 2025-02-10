using Application.Dtos.Todo;
using Domain.Models;

namespace Application.Interfaces;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetTodosAsync(TodoSearchDto searchDto);
    Task<Todo> GetTodoByIdAsync(Guid todoId);
    Task<Todo> AddTodoAsync(Todo todo);
    Task<Todo> UpdateTodoAsync(Guid id,Todo todo);
    Task<bool> DeleteTodoAsync(Guid id);
}   