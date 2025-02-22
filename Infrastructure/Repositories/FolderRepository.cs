using Application.Interfaces;
using Application.Options;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserIdentity _userIdentity;

    public FolderRepository(UserIdentity userIdentity, ApplicationDbContext context)
    {
        _userIdentity = userIdentity;
        _context = context;
    }

    public async Task<IEnumerable<Folder>> GetFoldersAsync()
    {
        var folders = await _context.Folders
            .Where( i => i.OwnerId == _userIdentity.Id)
            .ToListAsync();
        
        return folders;
    }

    public async Task<IEnumerable<Note>> GetNotesByFolderAsync(Guid folderId)
    {
        var folder = await _context.Folders
            .AnyAsync(f => f.Id == folderId && f.OwnerId == _userIdentity.Id);
            
        if(!folder)
            return Enumerable.Empty<Note>();
        
        return await _context.Notes
            .Where(f => f.FolderId == folderId && f.OwnerId == _userIdentity.Id)
            .ToListAsync();
    }

    public async Task<Folder> CreateFolderAsync(Folder folder)
    {
        var newFolder = new Folder()
        {
            Id = Guid.NewGuid(),
            Title = folder.Title,
            Description = folder.Description,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = folder.ModifiedAt,
            OwnerId = _userIdentity.Id
        };

        _context.Folders.Add(newFolder);
        await _context.SaveChangesAsync();

        return newFolder;
    }

    public async Task<Folder> UpdateFolderAsync(Guid folderId, Folder folder)
    {
        var folderToUpdate =await _context.Folders.FindAsync(folderId);

        if (folderToUpdate is null)
            return null;
        
        folderToUpdate.Title = folder.Title;
        folderToUpdate.Description = folder.Description;
        folderToUpdate.ModifiedAt = DateTime.UtcNow;
        
        _context.Folders.Update(folderToUpdate);
        await _context.SaveChangesAsync();

        return folderToUpdate;
    }

    public async Task<bool> DeleteFolderAsync(Guid folderId)
    {
        var folderToDelete = await _context.Folders.FindAsync(folderId);
        
        if (folderToDelete is null)
            return false;   
        
        _context.Folders.Remove(folderToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}