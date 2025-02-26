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

        var key = Encoding.UTF8.GetBytes(jwtSettings.Secret ?? throw new InvalidOperationException("JWT Secret is missing"));
        
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
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

        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", limiterOptions =>
            {
                limiterOptions.Window = TimeSpan.FromSeconds(10); // THIS DEFINES THE TIME WINDOW : EVERY 10 SECONDS THE LIMIT WILL RESET , ALLOWING FOR A NEW REQUESTS
                limiterOptions.PermitLimit = 10; // THIS DEFINES THE NUMBER OF REQUESTS ALLOWED IN THE TIME WINDOW
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // THIS DEFINES THE ORDER IN WHICH REQUESTS WILL BE PROCESSED
                limiterOptions.QueueLimit = 0; // THIS DEFINES HOW MANY EXTRA REQUESTS CAN BE QUEUED AFTER THE LIMIT HAS BEEN REACHED, THIS ALSO PREVENT ERROR 429
            });
        });
        
        
        return services;
    }
}
