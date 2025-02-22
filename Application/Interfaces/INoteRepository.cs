using Application.Dtos.Note;
using Domain.Models;

namespace Application.Interfaces;

public interface INoteRepository
{
    Task<IEnumerable<Note>> GetNotesAsync(NoteFilterCriteria filterCriteria);
    Task<Note> GetNoteByIdAsync(Guid noteId);
    Task<Note> AddNoteAsync(Note note);
    Task<Note> UpdateNoteAsync(Guid id, Note note);
    Task<bool> DeleteNoteAsync(Guid id);

}