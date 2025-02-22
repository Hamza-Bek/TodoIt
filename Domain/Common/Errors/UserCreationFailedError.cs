using System;
using FluentResults;

namespace Domain.Common.Errors;

public class UserCreationFailedError : Error
{
    public IEnumerable<string> Errors { get; set; }

    public UserCreationFailedError(IEnumerable<string> errors) : base("User creation failed")
    {
        Errors = errors;
    }
}
