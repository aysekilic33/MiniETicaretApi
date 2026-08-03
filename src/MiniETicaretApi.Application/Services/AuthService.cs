using Microsoft.EntityFrameworkCore;
using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;
using MiniETicaretApi.Application.Interfaces;
using MiniETicaretApi.Domain;

namespace MiniETicaretApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(IAppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        var mevcutKullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == request.Email);

        if (mevcutKullanici != null)
            return Result<AuthResponse>.Basarisiz("Bu email adresi zaten kayıtlı.");

        var kullanici = new Kullanici
        {
            AdSoyad = request.AdSoyad,
            Email = request.Email,
            SifreHash = BCrypt.Net.BCrypt.HashPassword(request.Sifre),
            Rol = KullaniciRolu.Musteri
        };

        _context.Kullanicilar.Add(kullanici);
        await _context.SaveChangesAsync();

        var token = _tokenService.TokenUret(kullanici);
        var response = new AuthResponse(token, kullanici.AdSoyad, kullanici.Email, kullanici.Rol.ToString());

        return Result<AuthResponse>.Basarili_(response);
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == request.Email);

        if (kullanici == null || !BCrypt.Net.BCrypt.Verify(request.Sifre, kullanici.SifreHash))
            return Result<AuthResponse>.Basarisiz("Email veya şifre hatalı.");

        var token = _tokenService.TokenUret(kullanici);
        var response = new AuthResponse(token, kullanici.AdSoyad, kullanici.Email, kullanici.Rol.ToString());

        return Result<AuthResponse>.Basarili_(response);
    }
}