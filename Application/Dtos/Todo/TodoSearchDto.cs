namespace Application.Dtos.Todo;

public class TodoSearchDto
{
    public bool Pinned { get; set; }
    public bool Completed { get; set; }
    public bool OverDue { get; set; }
    public bool TodosDay { get; set; }
}