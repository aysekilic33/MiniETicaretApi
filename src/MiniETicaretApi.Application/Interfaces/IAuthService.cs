using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;

namespace MiniETicaretApi.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
}