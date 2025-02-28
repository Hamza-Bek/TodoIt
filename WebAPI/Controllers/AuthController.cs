using System.Xml.XPath;
using Application.Dtos.Auth;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebAPI.Extensions;

namespace WebAPI.Controllers;

[EnableRateLimiting("anonymous")] 
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly IValidator<RegisterRequest> _registerRequestValidator;
    private readonly IValidator<RefreshTokenRequest> _refreshTokenRequestValidator;
    private readonly IValidator<RevokeTokenRequest> _revokeTokenRequestValidator;

    public AuthController(IAuthRepository authRepository, IValidator<LoginRequest> loginRequestValidator, IValidator<RegisterRequest> registerRequestValidator, IValidator<RefreshTokenRequest> refreshTokenRequestValidator, IValidator<RevokeTokenRequest> revokeTokenRequestValidator)
    {
        _authRepository = authRepository;
        _loginRequestValidator = loginRequestValidator;
        _registerRequestValidator = registerRequestValidator;
        _refreshTokenRequestValidator = refreshTokenRequestValidator;
        _revokeTokenRequestValidator = revokeTokenRequestValidator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _loginRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToValidationProblemDetails());

        var result = await _authRepository.LoginAsync(request, cancellationToken);
        return result.ToActionResult("Login successful", true);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _registerRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToValidationProblemDetails());

        var result = await _authRepository.RegisterAsync(request, cancellationToken);
        return result.ToActionResult("Register successful", true);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _refreshTokenRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToValidationProblemDetails());

        var result = await _authRepository.RefreshTokenAsync(request.Token, cancellationToken);
        return result.ToActionResult("Refresh succeeded", true);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _revokeTokenRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToValidationProblemDetails());

        var result = await _authRepository.RevokeTokenAsync(request.Token, cancellationToken);
        return result.ToActionResult("Revoke succeeded", true);
    }
}

