using Application.Dtos.Folder;

namespace Application.Services;

public class FolderService : IFolderService
{
    public Task<IEnumerable<FolderDto>> GetFoldersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<FolderDto> GetFolderByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<FolderDto> AddFolderAsync(FolderDto folder)
    {
        throw new NotImplementedException();
    }

    public Task<FolderDto> UpdateFolderAsync(Guid id, FolderDto folder)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFolderAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}