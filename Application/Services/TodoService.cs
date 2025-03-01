using System.Net.Http.Json;
using Application.Dtos.Todo;
using Application.Responses;
using Domain.Models;

namespace Application.Services;

public class TodoService : ITodoService
{
    private readonly HttpClient _httpClient;

    public TodoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IEnumerable<TodoDto>> GetTodosAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<TodoDto> GetTodoByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/Todos/get?id={id}");

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TodoDto>>();

        return apiResponse!.Data;
    }

    public async Task<TodoDto> AddTodoAsync(TodoDto todo)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Todos/add", todo);

        if (!response.IsSuccessStatusCode)
        {
            var errorReponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorReponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TodoDto>>();

        return apiResponse!.Data;
    }

    public async Task<TodoDto> UpdateTodoAsync(Guid id, TodoDto todo)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Todos/update?id={id}", todo);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TodoDto>>();
        return apiResponse!.Data;
    }

    public async Task DeleteTodoAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync("api/Todos/delete?id=" + id);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
    }
}