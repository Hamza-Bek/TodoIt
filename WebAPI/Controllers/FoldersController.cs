using Application.Dtos.Folder;
using Application.Interfaces;
using Application.Responses;
using Application.Mappers;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
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
        
        return Ok(new ApiResponse<IEnumerable<Folder>>()
        {
            Message = "Folders retrieved successfully",
            Succeeded = true,
            Data = folders
        });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetNotesByFolder(Guid folderId)
    {
        var notes = await _folderRepository.GetNotesByFolderAsync(folderId);
        
        return Ok(new ApiResponse<IEnumerable<Note>>()
        {
            Message = "Notes retrieved successfully",
            Succeeded = true,
            Data = notes
        });
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddFolder(FolderDto model)
    {
        var validationResult = await _folderValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiErrorResponse
            {
                ErrorMessage = validationResult.Errors.First().ErrorMessage
            });
        }
        
        var folder = await _folderRepository.CreateFolderAsync(model.ToModel());
        
        return Ok(new ApiResponse<FolderDto>()
        {
            Message = "Folder created successfully",
            Succeeded = true,
            Data = folder.ToDto()
        });
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateFolder(Guid folderId, FolderDto model)
    {
        var validationResult = await _folderValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiErrorResponse
            {
                ErrorMessage = validationResult.Errors.First().ErrorMessage
            });
        }
        
        var folder = await _folderRepository.UpdateFolderAsync(folderId, model.ToModel());
        
        return Ok(new ApiResponse<FolderDto>()
        {
            Message = "Folder updated successfully",
            Succeeded = true,
            Data = folder.ToDto()
        });
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteFolder(Guid folderId)
    {
        var result = await _folderRepository.DeleteFolderAsync(folderId);

        if (!result)
        {
            return NotFound(new ApiErrorResponse
            {
                ErrorMessage = "Folder not found"
            });
        }
        
        return Ok(new ApiResponse
        {
            Message = "Folder deleted successfully",
            Succeeded = true 
        });
    }
}