namespace Application.Responses;

public class ApiErrorResponse
{
    public string ErrorMessage { get; set; }
    public List<string>? Errors { get; set; }

    public ApiErrorResponse(string errorMessage, List<string>? errors = null)
    {
        ErrorMessage = errorMessage;
        Errors = errors;
    }
}

public class ApiErrorResponse<T> : ApiErrorResponse
{
    public T? Data { get; set; }

    public ApiErrorResponse(string errorMessage, T? data = default, List<string>? errors = null) : base(errorMessage, errors)
    {
        Data = data;
    }
}