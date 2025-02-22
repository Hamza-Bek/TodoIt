using System;
using Application.Dtos.Auth;
using FluentResults;

namespace Application.Interfaces;

public interface IAuthRepository
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RefreshTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<Result> RevokeTokenAsync(string token, CancellationToken cancellationToken = default);
}
