namespace MiniETicaretApi.Application.DTOs;

public record RegisterRequest(string AdSoyad, string Email, string Sifre);

public record LoginRequest(string Email, string Sifre);

public record AuthResponse(string Token, string AdSoyad, string Email, string Rol);