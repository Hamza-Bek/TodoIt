using Application.Dtos.Todo;
using Application.Interfaces;
using Application.Mappers;
using Application.Responses;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers;


[EnableRateLimiting("authenticated")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;
    private readonly IValidator<TodoDto> _todoValidator;

    public TodosController(ITodoRepository todoRepository, IValidator<TodoDto> todoValidator)
    {
        _todoRepository = todoRepository;
        _todoValidator = todoValidator;
    }
    
    [HttpGet("get/all")]
    public async Task<IActionResult> GetTodos([FromQuery] TodoFilterCriteria filterCriteria, CancellationToken cancellationToken)
    {
        var response = await _todoRepository.GetTodosAsync(filterCriteria, cancellationToken);

        return Ok(new ApiResponse<IEnumerable<Todo>>(
            "Todos retrieved successfully",
            true,
            response
        ));
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetTodoById([FromQuery]Guid todoId)
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
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTodo([FromBody] TodoDto model)
    {
        var validationResult = await _todoValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiErrorResponse(
                validationResult.Errors.First().ErrorMessage
            ));
        }

        var todo = await _todoRepository.AddTodoAsync(model.ToModel());

        return Ok(new ApiResponse<TodoDto>(
            "Todo added successfully",
            true,
            todo.ToDto()
        ));
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTodo([FromQuery]Guid id,[FromBody]TodoDto model)
    {
        var validationResult = await _todoValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiErrorResponse(
                validationResult.Errors.First().ErrorMessage
            ));
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

    [HttpDelete("delete")]
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