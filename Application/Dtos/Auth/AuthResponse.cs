using System;

namespace Application.Dtos.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpires { get; set; }
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];

    public AuthResponse(string token, string refreshToken, DateTime refreshTokenExpires, bool succeeded)
    {
        Token = token;
        RefreshToken = refreshToken;
        RefreshTokenExpires = refreshTokenExpires;
        Succeeded = succeeded;
    }

    public AuthResponse(IEnumerable<string> errors)
    {
        Errors = errors;
        Succeeded = false;
    }
}
