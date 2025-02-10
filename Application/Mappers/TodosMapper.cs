using Application.Dtos.Todo;
using Domain.Models;

namespace Application.Mappers;

public static class TodosMapper
{
    public static TodoDto ToDto (this Todo todo)
    {
        return new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            Completed = todo.Completed,
            CreatedAt = todo.CreatedAt,
            ModifiedAt = todo.ModifiedAt,
            Priority = todo.Priority,
            Pinned = todo.Pinned,
            Overdue = todo.Overdue
        };
    } 
}