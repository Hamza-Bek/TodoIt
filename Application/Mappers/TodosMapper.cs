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
    
    public static Todo ToModel(this TodoDto todoDto)
    {
        return new Todo
        {
            Id = todoDto.Id,
            Title = todoDto.Title,
            Description = todoDto.Description,
            Completed = todoDto.Completed,
            CreatedAt = todoDto.CreatedAt,
            ModifiedAt = todoDto.ModifiedAt,
            Priority = todoDto.Priority,
            Pinned = todoDto.Pinned,
            Overdue = todoDto.Overdue
        };
    }
}