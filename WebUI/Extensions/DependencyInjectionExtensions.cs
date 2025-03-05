using Application.Interfaces;
using Application.Services;

namespace WebUI.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IFolderService, FolderService>();
        
        return services;
    }
}