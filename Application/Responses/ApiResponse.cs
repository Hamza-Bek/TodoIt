namespace Application.Responses;

public class ApiResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Succeeded { get; set; }

    public ApiResponse(string message, bool succeeded)
    {
        Message = message;
        Succeeded = succeeded;
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponse(string message, bool succeeded, T? data = default) : base(message, succeeded)
    {
        Data = data;
    }
}