using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : ControllerBase
{
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetTodoById(Guid id)
    {
        return Ok();
    }
    
    [HttpGet("get/all")]
    public async Task<IActionResult> GetTodos()
    {
        return Ok();
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddTodo()
    {
        return Ok();
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateTodo(Guid id, Todo todo)
    {
        return Ok();
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        return Ok();
    }
}