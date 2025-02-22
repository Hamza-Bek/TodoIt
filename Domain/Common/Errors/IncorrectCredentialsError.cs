using FluentResults;

namespace Domain.Common.Errors;

public class IncorrectCredentialsError : Error
{
    public IncorrectCredentialsError() : base("Incorrect email or password")
    {
    }
}
