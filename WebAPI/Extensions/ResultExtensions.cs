using System;
using Application.Responses;
using Domain.Common.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Extensions;

/// <summary>
/// Extension methods for Result and Result<T> classes.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a Result<T> to an IActionResult.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <param name="successMessage">The message to include in the response if the result is successful.</param>
    public static IActionResult ToActionResult<T>(this Result<T> result, string successMessage, bool succeeded = true)
    {
        if (result.IsSuccess)
            return new OkObjectResult(new ApiResponse<T>(successMessage, succeeded, result.Value));

        var error = result.Errors.FirstOrDefault();
        if (error == null)
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        return HandleError(error);
    }

    /// <summary>
    /// Converts a Result to an IActionResult.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <param name="successMessage">The message to include in the response if the result is successful.</param>
    /// <param name="succeeded">Whether the result is successful.</param>
    /// <returns>The converted IActionResult.</returns>
    public static IActionResult ToActionResult(this Result result, string successMessage, bool succeeded = true)
    {
        if (result.IsSuccess)
            return new OkObjectResult(new ApiResponse(successMessage, succeeded));

        var error = result.Errors.FirstOrDefault();
        if (error == null)
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        return HandleError(error);
    }

    private static IActionResult HandleError(IError error)
    {
        // handle different error types here
        return error switch
        {
            UserLockedOutError e => new BadRequestObjectResult(new ApiErrorResponse(e.Message)),
            UserCreationFailedError e => new BadRequestObjectResult(new ApiErrorResponse(e.Message, e.Errors.ToList())),
            ValidationFailedError e => new BadRequestObjectResult(new ApiErrorResponse<ValidationFailedError>(e.Message, e)),
            UserNotFoundError e => new NotFoundObjectResult(new ApiErrorResponse(e.Message)),
            UserAlreadyExistsError e => new BadRequestObjectResult(new ApiErrorResponse(e.Message)),
            RefreshTokenError e => new BadRequestObjectResult(new ApiErrorResponse(e.Message)),
            IncorrectCredentialsError e => new BadRequestObjectResult(new ApiErrorResponse(e.Message)),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}
