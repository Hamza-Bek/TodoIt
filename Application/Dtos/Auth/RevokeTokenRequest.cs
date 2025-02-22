using System;

namespace Application.Dtos.Auth;

public class RevokeTokenRequest
{
    public string Token { get; set; } = string.Empty;
}
