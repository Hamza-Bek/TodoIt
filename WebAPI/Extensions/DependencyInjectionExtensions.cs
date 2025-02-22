using System.Security.Claims;
using Application.Dtos.Todo;
using Application.Interfaces;
using Application.Options;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
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

        var jwtSettings = new JwtSettings();
        configuration.GetSection("Jwt").Bind(jwtSettings);
        services.AddScoped(_ => jwtSettings);

        var refreshTokenSettings = new RefreshTokenSettings();
        configuration.GetSection("RefreshToken").Bind(refreshTokenSettings);
        services.AddScoped(_ => refreshTokenSettings);

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
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        services.AddScoped<ITokensService, TokensService>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        services.AddScoped<UserIdentity>(sp =>
        {
            var userIdentity = new UserIdentity();
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(userId, out var id))
                    userIdentity.Id = id;

            }
            return userIdentity;
        });
        return services;
    }
}
