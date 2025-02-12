using Domain.Models;

namespace Application.Interfaces;

public interface IFolderRepository
{
    Task<IEnumerable<Folder>> GetFoldersAsync();
    
    Task<IEnumerable<Note>> GetNotesByFolderAsync(Guid folderId);
    Task<Folder> CreateFolderAsync(Folder folder);
    Task<Folder> UpdateFolderAsync(Guid folderId, Folder folder);
    Task<bool> DeleteFolderAsync(Guid folderId);
}