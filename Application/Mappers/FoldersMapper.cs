using Application.Dtos.Folder;
using Domain.Models;

namespace Application.Mappers;

public static class FoldersMapper
{
    public static FolderDto ToDto(this Folder folder)
    {
        return new FolderDto
        {
            Id = folder.Id,
            Title = folder.Title,
            CreatedAt = folder.CreatedAt,
            ModifiedAt = folder.ModifiedAt
        };
    }
}