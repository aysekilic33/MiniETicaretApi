namespace MiniETicaretApi.Domain;

public class Siparis
{
    public int Id { get; set; }
    public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
    public SiparisDurumu Durum { get; set; } = SiparisDurumu.Beklemede;

    public int KullaniciId { get; set; }
    public Kullanici? Kullanici { get; set; }

    public ICollection<SiparisKalemi> Kalemler { get; set; } = new List<SiparisKalemi>();
}

public enum SiparisDurumu
{
    Beklemede = 0,
    Onaylandi = 1,
    Kargoya_Verildi = 2,
    Teslim_Edildi = 3,
    Iptal_Edildi = 4
}