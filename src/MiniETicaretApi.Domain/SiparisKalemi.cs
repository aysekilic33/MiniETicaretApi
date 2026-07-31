namespace MiniETicaretApi.Domain;

public class SiparisKalemi
{
    public int Id { get; set; }

    public int SiparisId { get; set; }
    public Siparis? Siparis { get; set; }

    public int UrunId { get; set; }
    public Urun? Urun { get; set; }

    public int Adet { get; set; }

    // Sipariş anındaki fiyatı burada saklıyoruz —
    // çünkü ürünün fiyatı ileride değişebilir,
    // ama geçmiş siparişteki fiyat sabit kalmalı.
    public decimal BirimFiyat { get; set; }
}