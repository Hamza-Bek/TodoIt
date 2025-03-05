using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Application.Dtos.Todo;
using Application.Interfaces;
using Application.Options;
using DefaultNamespace;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
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
        
        var jwtSecret = configuration["Jwt:Secret"];

        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new InvalidOperationException("JWT Secret key is missing!");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = configuration.GetValue<bool>("Jwt:ValidateIssuer"),
                    ValidateAudience = configuration.GetValue<bool>("Jwt:ValidateAudience"),
                    ValidateLifetime = configuration.GetValue<bool>("Jwt:ValidateLifetime"),
                    ValidateIssuerSigningKey = configuration.GetValue<bool>("Jwt:ValidateIssuerSigningKey"),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });
        
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
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        services.AddScoped<ITokensService, TokensService>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        
        services.AddValidatorsFromAssemblyContaining<TodoDtoValidator>();
         
        services.AddCustomRateLimiter();
        
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

        services.AddCors(options =>
        {
            options.AddPolicy("WebUI", policy =>
            {
                policy.WithOrigins("https://localhost:7222")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithHeaders(HeaderNames.ContentType);
            });
        });
        
        return services;
    }
}
