using Microsoft.EntityFrameworkCore;
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

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var mevcutKullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == request.Email);

        if (mevcutKullanici != null)
            throw new InvalidOperationException("Bu email adresi zaten kayıtlı.");

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

        return new AuthResponse(token, kullanici.AdSoyad, kullanici.Email, kullanici.Rol.ToString());
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == request.Email);

        if (kullanici == null || !BCrypt.Net.BCrypt.Verify(request.Sifre, kullanici.SifreHash))
            throw new UnauthorizedAccessException("Email veya şifre hatalı.");

        var token = _tokenService.TokenUret(kullanici);

        return new AuthResponse(token, kullanici.AdSoyad, kullanici.Email, kullanici.Rol.ToString());
    }
}