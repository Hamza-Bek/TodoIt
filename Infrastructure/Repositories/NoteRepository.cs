using Application.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    public Task<IEnumerable<Note>> GetNotesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Note> GetNoteByIdAsync(Guid noteId)
    {
        throw new NotImplementedException();
    }

    public Task<Note> AddNoteAsync(Note note)
    {
        throw new NotImplementedException();
    }

    public Task<Note> UpdateNoteAsync(Guid id, Note note)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteNoteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}