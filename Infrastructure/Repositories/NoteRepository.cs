using Application.Dtos.Note;
using Application.Interfaces;
using Application.Options;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserIdentity _userIdentity;

    public NoteRepository(ApplicationDbContext context, UserIdentity userIdentity)
    {
        _context = context;
        _userIdentity = userIdentity;
    }

    public async Task<IEnumerable<Note>> GetNotesAsync(NoteFilterCriteria filterCriteria)
    {
        var query = _context.Notes.Where(i => i.OwnerId == _userIdentity.Id).AsQueryable();
        
        if(filterCriteria.Title is not null)
            query = query.Where(i => i.Title.Contains(filterCriteria.Title));
        
        return await query.ToListAsync();
    }

    public async Task<Note> GetNoteByIdAsync(Guid id)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(i => i.Id == id && i.OwnerId == _userIdentity.Id);        

        return note;
    }


    public async Task<Note> AddNoteAsync(Note note)
    {
        var newNote = new Note
        {
            Id = Guid.NewGuid(),
            Title = note.Title,
            Content = note.Content,
            Pinned = false,
            CreatedAt = DateTime.UtcNow,
            OwnerId = _userIdentity.Id,
            FolderId = note.FolderId ?? null
        };
        
        _context.Notes.Add(newNote);
        await _context.SaveChangesAsync();

        return newNote;
    }

    public async Task<Note> UpdateNoteAsync(Guid id, Note note)
    { 
        var noteToUpdate = await _context.Notes.FindAsync(id);
        if (noteToUpdate is null)
            return null;
        
        noteToUpdate.Title = note.Title;
        noteToUpdate.Content = note.Content;
        noteToUpdate.Pinned = note.Pinned;
        noteToUpdate.FolderId = note.FolderId;

        await _context.SaveChangesAsync();

        return noteToUpdate;
    }

    public async Task<bool> DeleteNoteAsync(Guid id)
    {
        var noteToDelete = await _context.Notes.FindAsync(id);
        if (noteToDelete is null)
            return false;
        
        _context.Notes.Remove(noteToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}