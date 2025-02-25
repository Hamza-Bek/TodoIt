using System;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Extensions;

public static class ValidationResultExtensions
{
    public static ValidationProblemDetails ToValidationProblemDetails(this ValidationResult validationResult)
    {
        return new ValidationProblemDetails()
        {
            Title = "Validation failed",
            Status = StatusCodes.Status400BadRequest,
            Detail = "One or more validation errors occurred.",
            Extensions = { ["errors"] = validationResult.Errors.ToDictionary(
                e => e.PropertyName,
                e => new[] { e.ErrorMessage }
            ) }
        };
    }
}