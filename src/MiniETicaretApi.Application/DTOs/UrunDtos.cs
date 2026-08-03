namespace MiniETicaretApi.Application.DTOs;

public record UrunRequest(string Ad, string Aciklama, decimal Fiyat, int StokAdedi, int KategoriId);

public record UrunResponse(int Id, string Ad, string Aciklama, decimal Fiyat, int StokAdedi, int KategoriId, string KategoriAdi);