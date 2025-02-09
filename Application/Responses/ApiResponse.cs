namespace Application.Responses;

public class ApiResponse
{
    public required string Message { get; set; }
    public bool Succeeded { get; set; }
}

public class ApiResponse<T> : ApiResponse
{
    public T Data { get; set; } = default!;
}