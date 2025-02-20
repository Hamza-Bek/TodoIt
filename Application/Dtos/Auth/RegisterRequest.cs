using System;

namespace Application.Dtos.Auth;

public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool JoinNewsletter { get; set; } // for future use in the application
    public SessionDetails SessionDetails { get; set; } = new();
}
