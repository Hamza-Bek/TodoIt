using Application.Dtos.Note;
using Application.Interfaces;
using Application.Mappers;
using Application.Responses;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers;

[EnableRateLimiting("fixed")] 
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
  private readonly INoteRepository _noteRepository;
  private readonly IValidator<NoteDto> _noteValidator;

  public NotesController(INoteRepository noteRepository, IValidator<NoteDto> noteValidator)
  {
    _noteRepository = noteRepository;
    _noteValidator = noteValidator;
  }

  [HttpGet("get/all")]
  public async Task<IActionResult> GetAllNotes([FromQuery]NoteFilterCriteria filterCriteria)
  {
    var response = await _noteRepository.GetNotesAsync(filterCriteria);

    return Ok(new ApiResponse<IEnumerable<Note>>(
        "Notes retrieved successfully",
        true,
        response
    ));
  }

  [HttpGet("get")]
  public async Task<IActionResult> GetNoteById([FromQuery]Guid id)
  {
    var response = await _noteRepository.GetNoteByIdAsync(id);

    return Ok(new ApiResponse<Note>(
        "Note retrieved successfully",
        true,
        response
    ));
  }

  [HttpPost("add")]
  public async Task<IActionResult> AddNote(NoteDto model)
  {
    var validationResult = await _noteValidator.ValidateAsync(model);
    if (!validationResult.IsValid)
    {
      return BadRequest(new ApiErrorResponse(
          validationResult.Errors.First().ErrorMessage
      ));
    }

    var todo = await _noteRepository.AddNoteAsync(model.ToModel());

    return Ok(new ApiResponse<NoteDto>(
        "Note added successfully",
        true,
        todo.ToDto()
    ));
  }

  [HttpPut("update")]
  public async Task<IActionResult> UpdateNote([FromQuery]Guid id, NoteDto model)
  {
    var validationResult = await _noteValidator.ValidateAsync(model);
    if (!validationResult.IsValid)
    {
      return BadRequest(new ApiErrorResponse(
          validationResult.Errors.First().ErrorMessage
      ));
    }

    var response = await _noteRepository.UpdateNoteAsync(id, model.ToModel());

    return Ok(new ApiResponse<Note>(
        "Note updated successfully",
        true,
        response
    ));
  }

  [HttpDelete("delete")]
  public async Task<IActionResult> DeleteNote([FromQuery]Guid id)
  {
    var result = await _noteRepository.DeleteNoteAsync(id);

    if (!result)
    {
      return BadRequest(new ApiErrorResponse(
          "Note could not be deleted"
      ));
    }

    return Ok(new ApiResponse<Note>(
        "Note deleted successfully",
        true
    ));

  }
}