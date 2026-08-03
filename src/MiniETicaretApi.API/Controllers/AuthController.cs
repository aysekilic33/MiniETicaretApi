using Microsoft.AspNetCore.Mvc;
using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;
using MiniETicaretApi.Application.Interfaces;

namespace MiniETicaretApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        if (!result.Basarili)
            return BadRequest(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<AuthResponse>.Basarili_(result.Veri!));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.Basarili)
            return Unauthorized(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<AuthResponse>.Basarili_(result.Veri!));
    }
}