using System;

namespace Infrastructure.Options;

public class RefreshTokenSettings
{
    public int RefreshTokenLength { get; set; } = 256;
    public int LifetimeInDays { get; set; } = 30;
    public bool ReuseTokenAllowed { get; set; } = false;
}
