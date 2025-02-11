namespace Application.Dtos.Todo;

public class RescheduleTodosRequest
{
    public IEnumerable<Guid> Todos { get; set; } = [];
    public DateTime NewDate { get; set; }
}
