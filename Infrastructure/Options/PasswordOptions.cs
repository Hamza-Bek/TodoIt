using System;

namespace Infrastructure.Options;

public class PasswordOptions
{
    public bool RequireUppercase { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireDigit { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public int RequiredLength { get; set; }
    public int RequiredUniqueChars { get; set; }
}
