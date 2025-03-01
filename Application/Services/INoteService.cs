using Application.Dtos.Note;

namespace Application.Services;

public interface INoteService 
{
    Task<IEnumerable<NoteDto>> GetNotesAsync();
    Task<NoteDto> GetNoteByIdAsync(Guid id);
    Task<NoteDto> AddNoteAsync(NoteDto note);
    Task<NoteDto> UpdateNoteAsync(Guid id, NoteDto note);
    Task DeleteNoteAsync(Guid id);
}