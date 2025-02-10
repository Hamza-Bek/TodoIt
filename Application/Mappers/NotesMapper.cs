using Application.Dtos.Note;
using Domain.Models;

namespace Application.Mappers;

public static class NotesMapper
{
    public static NoteDto ToDto(this Note note)
    {
        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            ModifiedAt = note.ModifiedAt,
            Pinned = note.Pinned,
            FolderId = note.FolderId
        };
    }
}