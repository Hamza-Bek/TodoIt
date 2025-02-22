using System;

namespace Application.Dtos.Auth;

/// <summary>
/// Used to send details about the user's current session.
/// </summary>
public class SessionDetails
{
    public string? DeviceName { get; set; }
    public string? IpAddress { get; set; }
    public string? Platform { get; set; }
    public string? Browser { get; set; }
}
