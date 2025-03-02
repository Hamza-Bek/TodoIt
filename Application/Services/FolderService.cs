using System.Net.Http.Json;
using Application.Dtos.Folder;
using Application.Responses;

namespace Application.Services;

public class FolderService : IFolderService
{
    private readonly HttpClient _httpClient;

    public FolderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<FolderDto>> GetFoldersAsync()
    {
        var response = await _httpClient.GetAsync("api/Folders/get/all");
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<FolderDto>>>();
        
        return apiResponse!.Data;
    }

    public async Task<FolderDto> GetFolderByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync("api/Folders/get?id=" + id);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<FolderDto>>();
        
        return apiResponse!.Data;
    }

    public async Task<FolderDto> AddFolderAsync(FolderDto folder)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Folders/add", folder);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<FolderDto>>();
        
        return apiResponse!.Data;
    }

    public async Task<FolderDto> UpdateFolderAsync(Guid id, FolderDto folder)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Folders/update?id=" + id, folder);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<FolderDto>>();
        return apiResponse!.Data;
    }

    public async Task DeleteFolderAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync("api/Folders/delete?id=" + id);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new HttpRequestException(errorResponse?.ErrorMessage ?? "An unknown error occurred.");
        }
    }
}