using Microsoft.EntityFrameworkCore;
using MiniETicaretApi.Domain;

namespace MiniETicaretApi.Infrastructure;

public class AppDbContext : DbContext
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

        // Kullanıcı - Sipariş (1 kullanıcı -> çok sipariş)
        modelBuilder.Entity<Siparis>()
            .HasOne(s => s.Kullanici)
            .WithMany(k => k.Siparisler)
            .HasForeignKey(s => s.KullaniciId);

        // Siparis - SiparisKalemi (1 sipariş -> çok kalem)
        modelBuilder.Entity<SiparisKalemi>()
            .HasOne(sk => sk.Siparis)
            .WithMany(s => s.Kalemler)
            .HasForeignKey(sk => sk.SiparisId);

        // Urun - SiparisKalemi (1 ürün -> çok kalemde geçebilir)
        modelBuilder.Entity<SiparisKalemi>()
            .HasOne(sk => sk.Urun)
            .WithMany()
            .HasForeignKey(sk => sk.UrunId);

        // Kategori - Urun (1 kategori -> çok ürün)
        modelBuilder.Entity<Urun>()
            .HasOne(u => u.Kategori)
            .WithMany(k => k.Urunler)
            .HasForeignKey(u => u.KategoriId);

        // Email'in benzersiz (unique) olması
        modelBuilder.Entity<Kullanici>()
            .HasIndex(k => k.Email)
            .IsUnique();

        // Fiyat alanı için decimal hassasiyeti (SQL Server uyarısı almamak için)
        modelBuilder.Entity<Urun>()
            .Property(u => u.Fiyat)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SiparisKalemi>()
            .Property(sk => sk.BirimFiyat)
            .HasPrecision(18, 2);
    }
}