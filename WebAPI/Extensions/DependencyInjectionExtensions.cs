using Application.Dtos.Todo;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LockoutOptions = Infrastructure.Options.LockoutOptions;
using PasswordOptions = Infrastructure.Options.PasswordOptions;

namespace WebAPI.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Services from Infrastructure layer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = new DatabaseOptions();
        configuration.GetSection("Database").Bind(databaseOptions);

        var passwordOptions = new PasswordOptions();
        configuration.GetSection("Password").Bind(passwordOptions);

        var lockoutOptions = new LockoutOptions();
        configuration.GetSection("Lockout").Bind(lockoutOptions);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(databaseOptions.ConnectionString, b => b.MigrationsAssembly("WebAPI")));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Lockout.AllowedForNewUsers = lockoutOptions.AllowedForNewUsers;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutOptions.DefaultLockoutTimeSpan);
            options.Lockout.MaxFailedAccessAttempts = lockoutOptions.MaxFailedAccessAttempts;

            options.Password.RequireDigit = passwordOptions.RequireDigit;
            options.Password.RequireLowercase = passwordOptions.RequireLowercase;
            options.Password.RequireNonAlphanumeric = passwordOptions.RequireNonAlphanumeric;
            options.Password.RequireUppercase = passwordOptions.RequireUppercase;
            options.Password.RequiredLength = passwordOptions.RequiredLength;
            options.Password.RequiredUniqueChars = passwordOptions.RequiredUniqueChars;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    /// <summary>
    /// Services from Application layer
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITodoRepository, TodoRepository>();

        return services;
    }
}
