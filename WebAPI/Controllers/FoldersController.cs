using Application.Dtos.Folder;
using Application.Interfaces;
using Application.Responses;
using Application.Mappers;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers;

[EnableRateLimiting("fixed")] 
[ApiController]
[Route("api/[controller]")]
public class FoldersController : ControllerBase
{
    private readonly IFolderRepository _folderRepository;
    private readonly IValidator<FolderDto> _folderValidator;

    public FoldersController(IFolderRepository folderRepository, IValidator<FolderDto> folderValidator)
    {
        _folderRepository = folderRepository;
        _folderValidator = folderValidator;
    }

    [HttpGet("get/all")]
    public async Task<IActionResult> GetAllFolders()
    {
        var folders = await _folderRepository.GetFoldersAsync();

        return Ok(new ApiResponse<IEnumerable<Folder>>(
            "Folders retrieved successfully",
            true,
            folders
        ));
    }

    [HttpGet("get/notes")]
    public async Task<IActionResult> GetNotesByFolder(Guid id)
    {
        var notes = await _folderRepository.GetNotesByFolderAsync(id);

        return Ok(new ApiResponse<IEnumerable<Note>>(
            "Notes retrieved successfully",
            true,
            notes
        ));
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddFolder(FolderDto model)
    {
        var validationResult = await _folderValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiErrorResponse(
                validationResult.Errors.First().ErrorMessage
            ));
        }

        var folder = await _folderRepository.CreateFolderAsync(model.ToModel());

        return Ok(new ApiResponse<FolderDto>(
            "Folder created successfully",
            true,
            folder.ToDto()
        ));
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateFolder([FromQuery]Guid id, FolderDto model)
    {
        var validationResult = await _folderValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiErrorResponse(
                validationResult.Errors.First().ErrorMessage
            ));
        }

        var folder = await _folderRepository.UpdateFolderAsync(id, model.ToModel());

        return Ok(new ApiResponse<FolderDto>(
            "Folder updated successfully",
            true,
            folder.ToDto()
        ));
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFolder([FromQuery]Guid id)
    {
        var result = await _folderRepository.DeleteFolderAsync(id);

        if (!result)
        {
            return NotFound(new ApiErrorResponse(
                "Folder not found"
            ));
        }

        return Ok(new ApiResponse(
            "Folder deleted successfully",
            true
        ));
    }
}