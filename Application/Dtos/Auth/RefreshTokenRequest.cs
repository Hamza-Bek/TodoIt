using System;

namespace Application.Dtos.Auth;

public class RefreshTokenRequest
{
    public string Token { get; set; } = string.Empty;
}
