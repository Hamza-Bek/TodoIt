using System.Xml.XPath;
using Application.Dtos.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _authRepository.LoginAsync(request, cancellationToken);
        return result.ToActionResult("Login successful", true);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _authRepository.RegisterAsync(request, cancellationToken);
        return result.ToActionResult("Register successful", true);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _authRepository.RefreshTokenAsync(request.Token, cancellationToken);
        return result.ToActionResult("Refresh succeeded", true);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _authRepository.RevokeTokenAsync(request.Token, cancellationToken);
        return result.ToActionResult("Revoke succeeded", true);
    }
}

