using Application.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    public Task<IEnumerable<Todo>> GetTodosAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Todo> GetTodoByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Todo> AddTodoAsync(Todo todo)
    {
        throw new NotImplementedException();
    }

    public Task<Todo> UpdateTodoAsync(Guid id,Todo todo)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteTodoAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}