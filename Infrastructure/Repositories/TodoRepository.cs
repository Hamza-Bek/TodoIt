using Application.Dtos.Todo;
using Application.Interfaces;
using Application.Mappers;
using Application.Options;
using Domain.Enums;
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

    public async Task<IEnumerable<Todo>> GetTodosAsync(TodoFilterCriteria filter, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;

        var query = _context.Todos.AsQueryable();

        query = query.Where(i => i.OwnerId == _userIdentity.Id && i.DueDate.Date == filter.DueDate);

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(i => i.Title.Contains(filter.Title, StringComparison.OrdinalIgnoreCase));

        if (filter.Pinned)
            query = query.Where(i => i.Pinned == filter.Pinned);

        if (filter.Completed)
            query = query.Where(i => i.Completed == filter.Completed);

        if (filter.Overdue)
            query = query.Where(i => i.CreatedAt.Date < today && i.Completed == false);

        if (filter.Priority != null)
            query = query.Where(i => i.Priority == filter.Priority);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Todo> GetTodoByIdAsync(Guid todoId)
    {
        var todo = await _context.Todos
            .FirstOrDefaultAsync(i => i.Id == todoId && i.OwnerId == _userIdentity.Id);

        return todo!;
    }

    public async Task<Todo> AddTodoAsync(Todo todo)
    {
        
        var userId = _userIdentity.Id;
        
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException($"User is not authenticated. {userId}");
        }
        
        var endOfDay = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
        
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
            OwnerId = userId,
            DueDate = endOfDay
        };

        _context.Todos.Add(newTodo);
        await _context.SaveChangesAsync();

        return newTodo;
    }

    public async Task<Todo?> UpdateTodoAsync(Guid id, Todo todo)
    {
        var todoToUpdate = await _context.Todos.FindAsync(id);
        if (todoToUpdate is null)
            return null;

        todoToUpdate.Title = todo.Title;
        todoToUpdate.Description = todo.Description;
        todoToUpdate.Completed = todo.Completed;
        todoToUpdate.ModifiedAt = DateTime.UtcNow;
        todoToUpdate.Priority = todo.Priority;
        todoToUpdate.Pinned = todo.Pinned;
        todoToUpdate.Overdue = todo.Overdue;
        todoToUpdate.DueDate = todo.DueDate;

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

    public async Task RescheduleTodosAsync(IEnumerable<Guid> ids, DateTime newDate)
    {
        await _context.Todos
            .Where(t => ids.Contains(t.Id))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Overdue, false)
            .SetProperty(x => x.DueDate, newDate)
            .SetProperty(x => x.ModifiedAt, DateTime.UtcNow));
    }
}