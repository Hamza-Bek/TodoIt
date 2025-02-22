using FluentResults;

namespace Domain.Common.Errors;

public class UserLockedOutError : Error
{
    public UserLockedOutError(TimeSpan timeLeft)
        : base($"User is locked out. Try again in {timeLeft.TotalMinutes:F1} minutes.")
    {
        Metadata.Add("ErrorCode", "USER_LOCKED_OUT");
        Metadata.Add("TimeLeftSeconds", (int)timeLeft.TotalSeconds);
    }
}
