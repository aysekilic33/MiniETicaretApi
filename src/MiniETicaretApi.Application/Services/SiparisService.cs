using Microsoft.EntityFrameworkCore;
using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;
using MiniETicaretApi.Application.Interfaces;
using MiniETicaretApi.Domain;

namespace MiniETicaretApi.Application.Services;

public class SiparisService : ISiparisService
{
    private readonly IAppDbContext _context;

    public SiparisService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SiparisResponse>> OlusturAsync(int kullaniciId, SiparisOlusturRequest request)
    {
        if (request.Kalemler == null || request.Kalemler.Count == 0)
            return Result<SiparisResponse>.Basarisiz("Sipariş en az bir ürün içermelidir.");

        var siparis = new Siparis
        {
            KullaniciId = kullaniciId,
            Durum = SiparisDurumu.Beklemede,
            Kalemler = new List<SiparisKalemi>()
        };

        foreach (var kalemRequest in request.Kalemler)
        {
            var urun = await _context.Urunler.FindAsync(kalemRequest.UrunId);

            if (urun == null)
                return Result<SiparisResponse>.Basarisiz($"Ürün bulunamadı: Id={kalemRequest.UrunId}");

            if (urun.StokAdedi < kalemRequest.Adet)
                return Result<SiparisResponse>.Basarisiz($"'{urun.Ad}' için yeterli stok yok. Mevcut stok: {urun.StokAdedi}");

            urun.StokAdedi -= kalemRequest.Adet;

            siparis.Kalemler.Add(new SiparisKalemi
            {
                UrunId = urun.Id,
                Adet = kalemRequest.Adet,
                BirimFiyat = urun.Fiyat
            });
        }

        _context.Siparisler.Add(siparis);
        await _context.SaveChangesAsync();

        return await SiparisiResponseaCevirAsync(siparis.Id);
    }

    public async Task<Result<List<SiparisResponse>>> KullanicininSiparisleriniGetirAsync(int kullaniciId)
    {
        var siparisIdler = await _context.Siparisler
            .Where(s => s.KullaniciId == kullaniciId)
            .Select(s => s.Id)
            .ToListAsync();

        var sonuclar = new List<SiparisResponse>();
        foreach (var id in siparisIdler)
        {
            var result = await SiparisiResponseaCevirAsync(id);
            if (result.Basarili)
                sonuclar.Add(result.Veri!);
        }

        return Result<List<SiparisResponse>>.Basarili_(sonuclar);
    }

    public async Task<Result<SiparisResponse>> GetirAsync(int id, int kullaniciId, bool adminMi)
    {
        var siparis = await _context.Siparisler.FindAsync(id);

        if (siparis == null)
            return Result<SiparisResponse>.Basarisiz("Sipariş bulunamadı.");

        if (!adminMi && siparis.KullaniciId != kullaniciId)
            return Result<SiparisResponse>.Basarisiz("Bu siparişi görüntüleme yetkiniz yok.");

        return await SiparisiResponseaCevirAsync(id);
    }

    private async Task<Result<SiparisResponse>> SiparisiResponseaCevirAsync(int siparisId)
    {
        var siparis = await _context.Siparisler
            .Include(s => s.Kalemler)
            .ThenInclude(k => k.Urun)
            .FirstOrDefaultAsync(s => s.Id == siparisId);

        if (siparis == null)
            return Result<SiparisResponse>.Basarisiz("Sipariş bulunamadı.");

        var kalemler = siparis.Kalemler.Select(k => new SiparisKalemiResponse(
            k.UrunId,
            k.Urun!.Ad,
            k.Adet,
            k.BirimFiyat,
            k.Adet * k.BirimFiyat
        )).ToList();

        var toplamTutar = kalemler.Sum(k => k.ToplamFiyat);

        var response = new SiparisResponse(
            siparis.Id,
            siparis.OlusturmaTarihi,
            siparis.Durum.ToString(),
            toplamTutar,
            kalemler
        );

        return Result<SiparisResponse>.Basarili_(response);
    }
}