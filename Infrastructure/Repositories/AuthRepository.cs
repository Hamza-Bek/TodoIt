using System.Runtime.InteropServices;
using Application.Dtos.Auth;
using Application.Interfaces;
using Domain.Common.Errors;
using Domain.Models;
using FluentResults;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokensService _tokenService;
    private readonly IRefreshTokensRepository _refreshTokenRepository;
    private readonly RefreshTokenSettings _refreshTokenOptions;

    public AuthRepository(UserManager<ApplicationUser> userManager, ITokensService tokenService, IRefreshTokensRepository refreshTokenRepository, RefreshTokenSettings refreshTokenOptions)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenOptions = refreshTokenOptions;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result.Fail(new UserNotFoundError());

        // check if the user is locked out
        if (await _userManager.IsLockedOutAsync(user))
        {
            var timeLeft = user.LockoutEnd - DateTime.UtcNow;
            return Result.Fail(new UserLockedOutError(timeLeft!.Value));
        }

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isCorrectPassword)
        {
            await _userManager.AccessFailedAsync(user);
            return Result.Fail(new IncorrectCredentialsError());
        }


        var accessToken = _tokenService.GenerateJwtToken(user);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenOptions.LifetimeInDays),
            DeviceName = request.SessionDetails.DeviceName,
            Platform = request.SessionDetails.Platform,
            IpAddress = request.SessionDetails.IpAddress,
            Browser = request.SessionDetails.Browser
        };

        await _refreshTokenRepository.CreateAsync(refreshToken);
        var authResponse = new AuthResponse(accessToken, refreshToken.Token, refreshToken.ExpiresAt, true);

        return Result.Ok(authResponse);
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Result.Fail(new UserAlreadyExistsError());

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NormalizedEmail = request.Email.ToUpper(),
            NormalizedUserName = request.Email.ToUpper()
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Result.Fail(new UserCreationFailedError(result.Errors.Select(e => e.Description)));

        var accessToken = _tokenService.GenerateJwtToken(user);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenOptions.LifetimeInDays),
            DeviceName = request.SessionDetails.DeviceName,
            Platform = request.SessionDetails.Platform,
        };

        await _refreshTokenRepository.CreateAsync(refreshToken);
        var authResponse = new AuthResponse(accessToken, refreshToken.Token, refreshToken.ExpiresAt, true);

        return Result.Ok(authResponse);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
        if (refreshToken == null)
            return Result.Fail(new RefreshTokenError("Token was not found"));

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
            return Result.Fail(new RefreshTokenError("Refresh token is expired"));

        if (refreshToken.RevokedAt != null)
            return Result.Fail(new RefreshTokenError("Refresh token is revoked"));

        var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
        if (user == null)
            return Result.Fail(new UserNotFoundError());

        var accessToken = _tokenService.GenerateJwtToken(user);
        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenOptions.LifetimeInDays),
            DeviceName = refreshToken.DeviceName,
            Platform = refreshToken.Platform,
            IpAddress = refreshToken.IpAddress,
            Browser = refreshToken.Browser
        };

        // Delete old refresh token and add new one
        await _refreshTokenRepository.RevokeAsync(refreshToken);
        await _refreshTokenRepository.CreateAsync(newRefreshToken);

        var authResponse = new AuthResponse(accessToken, newRefreshToken.Token, newRefreshToken.ExpiresAt, true);
        return Result.Ok(authResponse);
    }

    public async Task<Result> RevokeTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
        if (refreshToken == null)
            return Result.Fail(new RefreshTokenError("Token was not found"));

        if (refreshToken.RevokedAt != null)
            return Result.Fail(new RefreshTokenError("Refresh token is already revoked"));

        var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
        if (user == null)
            return Result.Fail(new UserNotFoundError());

        await _refreshTokenRepository.RevokeAsync(refreshToken);
        return Result.Ok();
    }
}
