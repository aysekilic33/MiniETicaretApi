using Microsoft.EntityFrameworkCore;
using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;
using MiniETicaretApi.Application.Interfaces;
using MiniETicaretApi.Domain;

namespace MiniETicaretApi.Application.Services;

public class KategoriService : IKategoriService
{
    private readonly IAppDbContext _context;

    public KategoriService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<KategoriResponse>>> TumunuGetirAsync()
    {
        var kategoriler = await _context.Kategoriler
            .Select(k => new KategoriResponse(k.Id, k.Ad))
            .ToListAsync();

        return Result<List<KategoriResponse>>.Basarili_(kategoriler);
    }

    public async Task<Result<KategoriResponse>> GetirAsync(int id)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);

        if (kategori == null)
            return Result<KategoriResponse>.Basarisiz("Kategori bulunamadı.");

        return Result<KategoriResponse>.Basarili_(new KategoriResponse(kategori.Id, kategori.Ad));
    }

    public async Task<Result<KategoriResponse>> EkleAsync(KategoriRequest request)
    {
        var kategori = new Kategori { Ad = request.Ad };

        _context.Kategoriler.Add(kategori);
        await _context.SaveChangesAsync();

        return Result<KategoriResponse>.Basarili_(new KategoriResponse(kategori.Id, kategori.Ad));
    }

    public async Task<Result<KategoriResponse>> GuncelleAsync(int id, KategoriRequest request)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);

        if (kategori == null)
            return Result<KategoriResponse>.Basarisiz("Kategori bulunamadı.");

        kategori.Ad = request.Ad;
        await _context.SaveChangesAsync();

        return Result<KategoriResponse>.Basarili_(new KategoriResponse(kategori.Id, kategori.Ad));
    }

    public async Task<Result> SilAsync(int id)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);

        if (kategori == null)
            return Result.Basarisiz("Kategori bulunamadı.");

        _context.Kategoriler.Remove(kategori);
        await _context.SaveChangesAsync();

        return Result.Basarili_();
    }
}