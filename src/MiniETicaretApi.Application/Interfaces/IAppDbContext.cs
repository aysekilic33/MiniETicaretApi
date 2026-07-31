using Microsoft.EntityFrameworkCore;
using MiniETicaretApi.Domain;

namespace MiniETicaretApi.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Kullanici> Kullanicilar { get; }
    DbSet<Kategori> Kategoriler { get; }
    DbSet<Urun> Urunler { get; }
    DbSet<Siparis> Siparisler { get; }
    DbSet<SiparisKalemi> SiparisKalemleri { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}