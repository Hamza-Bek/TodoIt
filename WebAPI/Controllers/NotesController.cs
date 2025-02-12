using Application.Dtos.Note;
using Application.Interfaces;
using Application.Mappers;
using Application.Responses;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : Controller
{
    private readonly INoteRepository _noteRepository;

    public NotesController(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    [HttpGet("get/all")]
    public async Task<IActionResult> GetAllNotes(NoteFilterCriteria filterCriteria)
    {
        var response = await _noteRepository.GetNotesAsync(filterCriteria);
        
        return Ok(new ApiResponse<IEnumerable<Note>>
        {
          Message  = "Notes retrieved successfully",
          Succeeded = true,
          Data = response
        });
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetNoteById(Guid id)
    {
        var response = await _noteRepository.GetNoteByIdAsync(id);
        
        return Ok(new ApiResponse<Note>
        {
          Message  = "Note retrieved successfully",
          Succeeded = true,
          Data = response
        });
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddNote(NoteDto model)
    {
        var todo = await _noteRepository.AddNoteAsync(model.ToModel());

        return Ok(new ApiResponse<NoteDto>
        {
          Message  = "Note added successfully",
          Succeeded = true,
          Data = todo.ToDto()
        });
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateNote(Guid id, NoteDto model)
    {
        var response = await _noteRepository.UpdateNoteAsync(id, model.ToModel());
        
        return Ok(new ApiResponse<Note>
        {
          Message  = "Note updated successfully",
          Succeeded = true,
          Data = response
        });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        var result = await _noteRepository.DeleteNoteAsync(id);

        if (!result)
        {
            return BadRequest(new ApiErrorResponse
            {
              ErrorMessage  = "Note could not be deleted",
            });
        }
        
        return Ok(new ApiResponse<Note>
        {
          Message  = "Note deleted successfully",
          Succeeded = true
        });
        
    }
}