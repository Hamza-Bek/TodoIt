using Application.Dtos.Todo;
using Application.Interfaces;
using Application.Mappers;
using Application.Responses;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;
    private readonly IValidator<TodoDto> _todoValidator;

    public TodosController(ITodoRepository todoRepository, IValidator<TodoDto> todoValidator)
    {
        _todoRepository = todoRepository;
        _todoValidator = todoValidator;
    }

    [HttpPost("get/all")]
    public async Task<IActionResult> GetTodos([FromBody] TodoFilterCriteria filterCriteria, CancellationToken cancellationToken)
    {
        var respone = await _todoRepository.GetTodosAsync(filterCriteria, cancellationToken);

        return Ok(new ApiResponse<IEnumerable<Todo>>(
            "Todos retrieved successfully",
            true,
            respone
        ));
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetTodoById(Guid todoId)
    {
        var todo = await _todoRepository.GetTodoByIdAsync(todoId);

        if (todo == null)
        {
            return NotFound(new ApiErrorResponse(
                "Todo not found"
            ));
        }

        return Ok(new ApiResponse<Todo>(
            "Todo retrieved successfully",
            true,
            todo
        ));
        ;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTodo([FromBody] TodoDto model)
    {
        var validationResult = await _todoValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiErrorResponse
            {
                ErrorMessage = validationResult.Errors.First().ErrorMessage
            });
        }
        
        var todo = await _todoRepository.AddTodoAsync(model.ToModel());

        return Ok(new ApiResponse<TodoDto>(
            "Todo added successfully",
            true,
            todo.ToDto()
        ));
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateTodo(Guid id, TodoDto model)
    {
        var validationResult = await _todoValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiErrorResponse
            {
                ErrorMessage = validationResult.Errors.First().ErrorMessage
            });
        }
        
        var todo = await _todoRepository.UpdateTodoAsync(id, model.ToModel());

        if (todo == null)
            return NotFound(new ApiErrorResponse(
                "Todo not found"
            ));


        return Ok(new ApiResponse<TodoDto>(
            "Todo updated successfully",
            true,
            todo.ToDto()
        ));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        var response = await _todoRepository.DeleteTodoAsync(id);

        if (!response)
        {
            return NotFound(new ApiErrorResponse(
                "Todo not found"
            ));
        }

        return Ok(new ApiResponse(
            "Todo deleted successfully",
            true
        ));
    }

    [HttpPost("reschedule")]
    public async Task<IActionResult> RescheduleTodos([FromBody] RescheduleTodosRequest request)
    {
        await _todoRepository.RescheduleTodosAsync(request.Todos, request.NewDate);

        return Ok(new ApiResponse(
            "Todos rescheduled successfully",
            true
        ));
    }
}