using Domain.Enums;

namespace Application.Dtos.Todo;

public class TodoFilterCriteria
{
    public bool Pinned { get; set; }
    public bool Completed { get; set; }
    public bool Overdue { get; set; }
    public DateTime DueDate { get; set; } = DateTime.UtcNow.Date;
    public Priority? Priority { get; set; }
    public string Title { get; set; } = string.Empty;
}