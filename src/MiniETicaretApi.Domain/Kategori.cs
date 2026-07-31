namespace MiniETicaretApi.Domain;

public class Kategori
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;

    public ICollection<Urun> Urunler { get; set; } = new List<Urun>();
}