using System.Net.Http.Json;
using Application.Dtos.Note;
using Application.Responses;

namespace Application.Services;

public class NoteService : INoteService
{
    private readonly HttpClient _httpClient;

    public NoteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<NoteDto>> GetNotesAsync()
    {
        var response = await _httpClient.GetAsync("api/Notes/get/all");

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<NoteDto>>>();
        return apiResponse!.Data;
    }

    public async Task<NoteDto> GetNoteByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync("api/Notes/get?id=" + id);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync < ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<NoteDto>>();
        return apiResponse!.Data;
    }

    public async Task<NoteDto> AddNoteAsync(NoteDto note)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Notes/add", note);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<NoteDto>>();
        return apiResponse!.Data;
    }

    public async Task<NoteDto> UpdateNoteAsync(Guid id, NoteDto note)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Notes/update?id=" + id, note);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<NoteDto>>();
        return apiResponse!.Data;
    }

    public async Task DeleteNoteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync("api/Notes/delete?id=" + id);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
    }
}