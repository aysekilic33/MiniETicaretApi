using Microsoft.EntityFrameworkCore;
using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;
using MiniETicaretApi.Application.Interfaces;
using MiniETicaretApi.Domain;

namespace MiniETicaretApi.Application.Services;

public class UrunService : IUrunService
{
    private readonly IAppDbContext _context;

    public UrunService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<UrunResponse>>> TumunuGetirAsync(int? kategoriId = null)
    {
        var query = _context.Urunler.Include(u => u.Kategori).AsQueryable();

        if (kategoriId.HasValue)
            query = query.Where(u => u.KategoriId == kategoriId.Value);

        var urunler = await query
            .Select(u => new UrunResponse(u.Id, u.Ad, u.Aciklama, u.Fiyat, u.StokAdedi, u.KategoriId, u.Kategori!.Ad))
            .ToListAsync();

        return Result<List<UrunResponse>>.Basarili_(urunler);
    }

    public async Task<Result<UrunResponse>> GetirAsync(int id)
    {
        var urun = await _context.Urunler.Include(u => u.Kategori)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (urun == null)
            return Result<UrunResponse>.Basarisiz("Ürün bulunamadı.");

        return Result<UrunResponse>.Basarili_(new UrunResponse(urun.Id, urun.Ad, urun.Aciklama, urun.Fiyat, urun.StokAdedi, urun.KategoriId, urun.Kategori!.Ad));
    }

    public async Task<Result<UrunResponse>> EkleAsync(UrunRequest request)
    {
        var kategori = await _context.Kategoriler.FindAsync(request.KategoriId);
        if (kategori == null)
            return Result<UrunResponse>.Basarisiz("Belirtilen kategori bulunamadı.");

        var urun = new Urun
        {
            Ad = request.Ad,
            Aciklama = request.Aciklama,
            Fiyat = request.Fiyat,
            StokAdedi = request.StokAdedi,
            KategoriId = request.KategoriId
        };

        _context.Urunler.Add(urun);
        await _context.SaveChangesAsync();

        return Result<UrunResponse>.Basarili_(new UrunResponse(urun.Id, urun.Ad, urun.Aciklama, urun.Fiyat, urun.StokAdedi, urun.KategoriId, kategori.Ad));
    }

    public async Task<Result<UrunResponse>> GuncelleAsync(int id, UrunRequest request)
    {
        var urun = await _context.Urunler.FindAsync(id);
        if (urun == null)
            return Result<UrunResponse>.Basarisiz("Ürün bulunamadı.");

        var kategori = await _context.Kategoriler.FindAsync(request.KategoriId);
        if (kategori == null)
            return Result<UrunResponse>.Basarisiz("Belirtilen kategori bulunamadı.");

        urun.Ad = request.Ad;
        urun.Aciklama = request.Aciklama;
        urun.Fiyat = request.Fiyat;
        urun.StokAdedi = request.StokAdedi;
        urun.KategoriId = request.KategoriId;

        await _context.SaveChangesAsync();

        return Result<UrunResponse>.Basarili_(new UrunResponse(urun.Id, urun.Ad, urun.Aciklama, urun.Fiyat, urun.StokAdedi, urun.KategoriId, kategori.Ad));
    }

    public async Task<Result> SilAsync(int id)
    {
        var urun = await _context.Urunler.FindAsync(id);
        if (urun == null)
            return Result.Basarisiz("Ürün bulunamadı.");

        _context.Urunler.Remove(urun);
        await _context.SaveChangesAsync();

        return Result.Basarili_();
    }
}