namespace MiniETicaretApi.Domain;

public class Urun
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Aciklama { get; set; } = string.Empty;
    public decimal Fiyat { get; set; }
    public int StokAdedi { get; set; }

    public int KategoriId { get; set; }
    public Kategori? Kategori { get; set; }
}