namespace MiniETicaretApi.Domain;

public class Kullanici
{
    public int Id { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SifreHash { get; set; } = string.Empty;
    public KullaniciRolu Rol { get; set; } = KullaniciRolu.Musteri;
    public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

    public ICollection<Siparis> Siparisler { get; set; } = new List<Siparis>();
}

public enum KullaniciRolu
{
    Musteri = 0,
    Admin = 1
}