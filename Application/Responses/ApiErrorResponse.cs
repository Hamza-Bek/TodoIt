namespace Application.Responses;

public class ApiErrorResponse
{
    public required string ErrorMessage { get; set; }
    public List<string>? Errors { get; set; }
}