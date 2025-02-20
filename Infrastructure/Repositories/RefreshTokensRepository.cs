using System;
using Application.Interfaces;
using Application.Options;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserIdentity _userIdentity;
    private readonly ILogger<RefreshTokensRepository> _logger;

    public RefreshTokensRepository(ApplicationDbContext context, UserIdentity userIdentity, ILogger<RefreshTokensRepository> logger)
    {
        _context = context;
        _userIdentity = userIdentity;
        _logger = logger;
    }

    public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<bool> DeleteRefreshTokenAsync(Guid id)
    {
        var token = await _context.RefreshTokens.FindAsync(id);
        if (token == null)
        {
            _logger.LogWarning("Refresh token with id {Id} not found", id);
            return false;
        }

        _context.RefreshTokens.Remove(token);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(Guid id)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Id == id);

        return token;
    }

    public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task<IEnumerable<RefreshToken>> GetUserRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);

        return tokens;
    }
}
