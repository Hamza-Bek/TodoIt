using System;
using Domain.Models;

namespace Application.Interfaces;

public interface ITokensService
{
    string GenerateJwtToken(ApplicationUser user);
    string GenerateRefreshToken();
}
