using System;
using Domain.Models;

namespace Application.Interfaces;

public interface IRefreshTokensRepository
{
    Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenAsync(Guid id);
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
    Task<bool> DeleteRefreshTokenAsync(Guid id);
    Task<IEnumerable<RefreshToken>> GetUserRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default); // used to retrieve user sessions across devices
}
