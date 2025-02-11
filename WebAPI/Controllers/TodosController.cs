using Application.Dtos.Todo;
using Application.Interfaces;
using Application.Mappers;
using Application.Responses;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;

    public TodosController(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [HttpPost("get/all")]
    public async Task<IActionResult> GetTodos([FromBody] TodoFilterCriteria filterCriteria, CancellationToken cancellationToken)
    {
        var respone = await _todoRepository.GetTodosAsync(filterCriteria, cancellationToken);

        return Ok(new ApiResponse<IEnumerable<Todo>>()
        {
            Message = "Todos retrieved successfully",
            Succeeded = true,
            Data = respone
        });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetTodoById(Guid todoId)
    {
        var todo = await _todoRepository.GetTodoByIdAsync(todoId);

        if (todo == null)
        {
            return NotFound(new ApiErrorResponse
            {
                ErrorMessage = "Todo not found"
            });
        }

        return Ok(new ApiResponse<Todo>
        {
            Message = "Todo retrieved successfully",
            Succeeded = true,
            Data = todo
        });
        ;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTodo([FromBody] TodoDto model)
    {
        var todo = await _todoRepository.AddTodoAsync(model.ToModel());

        return Ok(new ApiResponse<TodoDto>
        {
            Message = "Todo added successfully",
            Succeeded = true,
            Data = todo.ToDto()
        });
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateTodo(Guid id, TodoDto model)
    {
        var todo = await _todoRepository.UpdateTodoAsync(id, model.ToModel());

        return Ok(new ApiResponse<TodoDto>
        {
            Message = "Todo updated successfully",
            Succeeded = true,
            Data = todo.ToDto()
        });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        var response = await _todoRepository.DeleteTodoAsync(id);

        if (!response)
        {
            return NotFound(new ApiErrorResponse
            {
                ErrorMessage = "Todo not found"
            });
        }

        return Ok(new ApiResponse
        {
            Message = "Todo deleted successfully",
            Succeeded = true
        });
    }

    [HttpPost("reschedule")]
    public async Task<IActionResult> RescheduleTodos([FromBody] RescheduleTodosRequest request)
    {
        await _todoRepository.RescheduleTodosAsync(request.Todos, request.NewDate);

        return Ok(new ApiResponse
        {
            Message = "Todos rescheduled successfully",
            Succeeded = true
        });
    }
}