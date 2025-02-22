using System;
using FluentResults;

namespace Domain.Common.Errors;

public class RefreshTokenError : Error
{
    public RefreshTokenError(string message) : base(message)
    {
    }
}
