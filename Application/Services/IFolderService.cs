using Application.Dtos.Folder;

namespace Application.Services;

public interface IFolderService
{
    Task<IEnumerable<FolderDto>> GetFoldersAsync();
    Task<FolderDto> GetFolderByIdAsync(Guid id);
    Task<FolderDto> AddFolderAsync(FolderDto folder);
    Task<FolderDto> UpdateFolderAsync(Guid id, FolderDto folder);
    Task DeleteFolderAsync(Guid id);
}