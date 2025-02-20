using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class RefreshToken : EntityBase
{
    public Guid UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime Expires { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByToken { get; set; }

    public bool Used { get; set; }

    public string? JwtId { get; set; }

    public string? DeviceName { get; set; }

    public string? Platform { get; set; }

    public string? IpAddress { get; set; }

    public string? Browser { get; set; }

    public bool IsRevoked => RevokedAt.HasValue;
}
