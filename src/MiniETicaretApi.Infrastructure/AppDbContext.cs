using Microsoft.EntityFrameworkCore;
using MiniETicaretApi.Application.Interfaces;
using MiniETicaretApi.Domain;

namespace MiniETicaretApi.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
    public DbSet<Kategori> Kategoriler => Set<Kategori>();
    public DbSet<Urun> Urunler => Set<Urun>();
    public DbSet<Siparis> Siparisler => Set<Siparis>();
    public DbSet<SiparisKalemi> SiparisKalemleri => Set<SiparisKalemi>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Siparis>()
            .HasOne(s => s.Kullanici)
            .WithMany(k => k.Siparisler)
            .HasForeignKey(s => s.KullaniciId);

        modelBuilder.Entity<SiparisKalemi>()
            .HasOne(sk => sk.Siparis)
            .WithMany(s => s.Kalemler)
            .HasForeignKey(sk => sk.SiparisId);

        modelBuilder.Entity<SiparisKalemi>()
            .HasOne(sk => sk.Urun)
            .WithMany()
            .HasForeignKey(sk => sk.UrunId);

        modelBuilder.Entity<Urun>()
            .HasOne(u => u.Kategori)
            .WithMany(k => k.Urunler)
            .HasForeignKey(u => u.KategoriId);

        modelBuilder.Entity<Kullanici>()
            .HasIndex(k => k.Email)
            .IsUnique();

        modelBuilder.Entity<Urun>()
            .Property(u => u.Fiyat)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SiparisKalemi>()
            .Property(sk => sk.BirimFiyat)
            .HasPrecision(18, 2);
    }
}