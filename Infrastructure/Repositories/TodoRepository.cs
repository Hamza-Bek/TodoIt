using Application.Dtos.Todo;
using Application.Interfaces;
using Application.Mappers;
using Application.Options;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserIdentity _userIdentity;
    public TodoRepository(ApplicationDbContext context, UserIdentity userIdentity)
    {
        _context = context;
        _userIdentity = userIdentity;
    }

    public async Task<IEnumerable<Todo>> GetTodosAsync()
    {
        var todos = await _context.Todos
            .Where(i => i.OwnerId == _userIdentity.Id)
            .ToListAsync();
        
        return todos;
    }

    public async Task<Todo> GetTodoByIdAsync(Guid todoId)
    {
        var todo = await _context.Todos
            .FirstOrDefaultAsync(i => i.Id == todoId && i.OwnerId == _userIdentity.Id);

        return todo!;
    }

    public async Task<Todo> AddTodoAsync(Todo todo)
    {
        var newTodo = new Todo
        {
            Id = Guid.NewGuid(),
            Title = todo.Title,
            Description = todo.Description,
            Completed = todo.Completed,
            CreatedAt = DateTime.UtcNow,
            Priority = todo.Priority,
            Pinned = todo.Pinned,
            Overdue = todo.Overdue,
            OwnerId = _userIdentity.Id
        };

        _context.Todos.Add(newTodo);
        await _context.SaveChangesAsync();

        return newTodo;
    }

    public async Task<Todo> UpdateTodoAsync(Guid id,Todo todo)
    {
        var todoToUpdate = await _context.Todos.FindAsync(id);
        if (todoToUpdate is not null)
            return null!;

        todoToUpdate.Title = todo.Title;
        todoToUpdate.Description = todo.Description;
        todoToUpdate.Completed = todo.Completed;
        todoToUpdate.ModifiedAt = DateTime.UtcNow;
        todoToUpdate.Priority = todo.Priority;
        todoToUpdate.Pinned = todo.Pinned;
        todoToUpdate.Overdue = todo.Overdue;

        await _context.SaveChangesAsync();

        return todoToUpdate;
    }
    
    public async Task<bool> DeleteTodoAsync(Guid id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo is null)
            return false;
        
        _context.Todos.Remove(todo); 
        await _context.SaveChangesAsync(); 
        return true;
    }
}